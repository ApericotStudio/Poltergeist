using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 Move;
		public Vector2 Look;
		public bool Jump;
		public bool Sprint;
		public float Fly;

		[Header("Movement Settings")]
		public bool AnalogMovement;

		[Header("Mouse Cursor Settings")]
		public bool CursorLocked = true;
		public bool CursorInputForLook = true;

		//events
		[SerializeField] private UnityEvent _onCancelInput;
		[SerializeField] private UnityEvent _onInteractPossessInput;
		[SerializeField] private UnityEvent _onUnpossessInput;
		[SerializeField] private UnityEvent _onAimThrowInput;
		[SerializeField] private UnityEvent _onThrowInput;
        [SerializeField] private UnityEvent _onPolterSenseEnterInput;
        [SerializeField] private UnityEvent _onPolterSenseLeaveInput;
		[SerializeField] private UnityEvent _onConsoleToggleInput;
        [SerializeField] private UnityEvent _onOptionsInput;
        [SerializeField] private UnityEvent _onGhostFaceChangeInput;
		[SerializeField] private UnityEvent _onSkipInput;

		public UnityEvent OnCancelInput { get => _onCancelInput; set => _onCancelInput = value; }
		public UnityEvent OnInteractPossessInput { get => _onInteractPossessInput; set => _onInteractPossessInput = value; }
		public UnityEvent OnUnpossessInput { get => _onUnpossessInput; set => _onUnpossessInput = value; }
		public UnityEvent OnAimThrowInput { get => _onAimThrowInput; set => _onAimThrowInput = value; }
		public UnityEvent OnThrowInput { get => _onThrowInput; set => _onThrowInput = value; }
		public UnityEvent OnConsoleToggleInput { get => _onConsoleToggleInput; set => _onConsoleToggleInput = value; }
		public UnityEvent OnGhostFaceChangeInput { get => _onGhostFaceChangeInput; set => _onGhostFaceChangeInput = value; }
        public UnityEvent OnOptionsInput { get => _onOptionsInput; set => _onOptionsInput = value; }
        public UnityEvent OnSkipInput { get => _onSkipInput; set => _onSkipInput = value; }

#if ENABLE_INPUT_SYSTEM
        private void OnMove(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				MoveInput(value.Get<Vector2>());
			}
		}

		private void OnFly(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				FlyInput(value.Get<float>());
			}
		}

		private void OnLook(InputValue value)
		{
			if (CursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		private void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		private void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		private void OnCancel(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				_onCancelInput.Invoke();
			}
		}

		private void OnInteractPossess(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				_onInteractPossessInput.Invoke();
			}
		}

		private void OnAimThrow(InputValue value)
        {
			if (Time.timeScale > 0)
			{
				_onAimThrowInput.Invoke();
			}
        }

		private void OnThrow(InputValue value)
        {
			if (Time.timeScale > 0)
			{
				_onThrowInput.Invoke();
			}
        }

		private void OnUnpossess(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				_onUnpossessInput.Invoke();
			}
		}

		private void OnPolterSenseEnter(InputValue value)
		{
			if (Time.timeScale > 0)
            {
				_onPolterSenseEnterInput.Invoke();
			}
		}

		private void OnPolterSenseLeave(InputValue value)
		{
			if (Time.timeScale > 0)
			{
				_onPolterSenseLeaveInput.Invoke();
			}
		}

		private void OnGhostFaceChange(InputValue value)
        {
			_onGhostFaceChangeInput.Invoke();
        }

		private void OnConsoleToggle(InputValue value)
        {
			_onConsoleToggleInput.Invoke();
        }

        private void OnOptions(InputValue value)
        {
            _onOptionsInput.Invoke();
        }

		private void OnSkip(InputValue value)
		{
			_onSkipInput.Invoke();
		}
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		}

		public void FlyInput(float newFlyDirection)
		{
			Fly = newFlyDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			Jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			Sprint = newSprintState;
		}
	}
}