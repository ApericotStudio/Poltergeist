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

		public UnityEvent OnCancelInput { get => _onCancelInput; set => _onCancelInput = value; }
		public UnityEvent OnInteractPossessInput { get => _onInteractPossessInput; set => _onInteractPossessInput = value; }
		public UnityEvent OnUnpossessInput { get => _onUnpossessInput; set => _onUnpossessInput = value; }
		public UnityEvent OnAimThrowInput { get => _onAimThrowInput; set => _onAimThrowInput = value; }
		public UnityEvent OnThrowInput { get => _onThrowInput; set => _onThrowInput = value; }

#if ENABLE_INPUT_SYSTEM
        private void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		private void OnFly(InputValue value)
		{
			FlyInput(value.Get<float>());
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
			_onCancelInput.Invoke();
		}

		private void OnInteractPossess(InputValue value)
		{
			_onInteractPossessInput.Invoke();
		}

		private void OnAimThrow(InputValue value)
        {
			_onAimThrowInput.Invoke();
        }

		private void OnThrow(InputValue value)
        {
			_onThrowInput.Invoke();
        }

		private void OnUnpossess(InputValue value)
		{
			_onUnpossessInput.Invoke();
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

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(CursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}