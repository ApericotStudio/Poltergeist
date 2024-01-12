﻿using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.AI;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private float _moveSpeed = 5.0f;
        [SerializeField] private float _flySpeed = 5.0f;
        [SerializeField] private float _aimSpeed = 2.0f;

        [Tooltip("How fast the player goes from moving to stopping, lower values are faster")]
        [Range(0.0f, 10f)]
        [SerializeField] private float _stoppingSpeed = 0.047f;

        [Tooltip("How fast the player goes from not moving to moving, lower values are faster")]
        [Range(0.0f, 0.2f)]
        [SerializeField] private float _movingSpeed = 0.038f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float SpeedChangeRate = 10.0f;
        [SerializeField] private float Sensitivity = 1f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField] private GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        [SerializeField] private float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        [SerializeField] private float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        [SerializeField] private float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        [SerializeField]private bool LockCameraPosition = false;

        [Header("Flying")]
        public float MinHeight;
        public float MaxHeight;
        public float CurrentHeight;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        public bool freeze;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private PossessionController _posControl;
        private GameObject _mainCamera;
        private SkinnedMeshRenderer[] _meshRs;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private float _targetSpeed;
        private bool _aim;
        private Vector3 _previousMovement;
        private Vector3 _newMovement;
        private bool _tutorialShown = false;

        public delegate void Movement(int index);
        public event Movement hasMoved;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            if(SceneManager.GetActiveScene().name != "Assignment")
            {
                _tutorialShown = true;
            }

            int playerLayer = LayerMask.NameToLayer("Player");
            int npcObjectLayer = LayerMask.NameToLayer("Npc");
            int cameraObjectLayer = LayerMask.NameToLayer("Camera");

            Physics.IgnoreLayerCollision(playerLayer, npcObjectLayer);
            Physics.IgnoreLayerCollision(playerLayer, cameraObjectLayer);

            Sensitivity = PlayerPrefs.GetFloat(PlayerPrefsVariable.Sensitivity.ToString(), 1);
        }

        private void Start()
        {
            CurrentHeight = transform.position.y;
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _animator = GetComponentInChildren<Animator>();
            _hasAnimator = _animator != null;
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
            _posControl = gameObject.GetComponent<PossessionController>();
            _posControl.CurrentPossessionChanged.AddListener(TogglePlayerVisible);
            _meshRs = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AssignAnimationIDs();
        }

        private void Update()
        {
            GroundedCheck();
            if (!freeze && Time.timeScale > 0)
            {
                Move();
            }
        }

        private void LateUpdate()
        {
            if(Time.deltaTime > 0)
            {
                CameraRotation();
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier * Sensitivity;
                _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier * Sensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        public void SetSensitivity (float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            if (_aim)
            {
                _targetSpeed = _aimSpeed;
            }
            else
            {
                _targetSpeed = _moveSpeed;
            }

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.Move == Vector2.zero) _targetSpeed = 0.0f;

            float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;



            _speed = Mathf.Lerp(_speed, _targetSpeed, _movingSpeed);

            _animationBlend = Mathf.Lerp(_animationBlend, _targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.Move.x, _input.Fly, _input.Move.y).normalized;

            if (_input.Move != Vector2.zero && !_tutorialShown)
            {
                _tutorialShown = true;
                hasMoved?.Invoke(1);
            }

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);



                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // changes player input based on camera orientation
            Vector3 targetRight = _input.Move.x * _mainCamera.transform.right;
            Vector3 targetForward = _input.Move.y * _mainCamera.transform.forward;

            Vector3 targetDirection = (targetRight + targetForward).normalized;

            if (inputDirection.y != 0.0f)
            {
                targetDirection.y = 0.0f;
            }

            // move the player
            _newMovement = (targetDirection * _speed + new Vector3(0.0f, inputDirection.y, 0.0f) * _flySpeed) * Time.deltaTime;

            if (_input.Move == Vector2.zero)
            {
                // after stopping with flying lerp in y direction
                if (_input.Fly == 0)
                {
                    _newMovement = Vector3.Lerp(_previousMovement, Vector3.zero, _stoppingSpeed * Time.deltaTime);
                } 
                // after stopping with walking lerp in x, y direction
                else
                {
                    _newMovement = Vector3.Lerp(new Vector3(_previousMovement.x, _newMovement.y, _previousMovement.z), new Vector3(0, _newMovement.y, 0), _stoppingSpeed * Time.deltaTime);
                }
                
            }

            _previousMovement = _newMovement;

            _controller.Move(_newMovement);
            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public void AimEnter()
        {
            _aim = true;
        }

        public void AimExit()
        {
            _aim = false;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void ToUnpossessLocation()
        {
            Ray ray = new(_posControl.CurrentPossession.transform.position, Vector3.down);
            Physics.Raycast(ray, out RaycastHit hitInfo);
            _controller.enabled = false;
            if (NavMesh.SamplePosition(hitInfo.point, out NavMeshHit hit, 2f, 1))
            { gameObject.transform.position = hit.position; }
            else if (NavMesh.SamplePosition(hitInfo.point, out hit, 5f, 1))
            { gameObject.transform.position = hit.position; }
            _controller.enabled = true;
        }

        public void TogglePlayerVisible()
        {
            foreach (SkinnedMeshRenderer mesh in _meshRs) { mesh.enabled = _posControl.CurrentPossession == null; }
        }
    }
}