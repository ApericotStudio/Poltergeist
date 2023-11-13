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
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public float fly;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		//events
		[SerializeField] private UnityEvent _onPressAimInput;
		[SerializeField] private UnityEvent _onCancelAimInput;
		[SerializeField] private UnityEvent _onConfirmAimInput;

		public UnityEvent OnPressAimInput { get => _onPressAimInput; set => _onPressAimInput = value; }
		public UnityEvent OnCancelAimInput { get => _onCancelAimInput; set => _onCancelAimInput = value; }
		public UnityEvent OnConfirmAimInput { get => _onConfirmAimInput; set => _onConfirmAimInput = value; }

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
			if (cursorInputForLook)
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

		private void OnAim(InputValue value)
		{
			_onPressAimInput.Invoke();
		}

		private void OnAimCancel(InputValue value)
		{
			_onCancelAimInput.Invoke();
		}

		private void OnAimConfirm(InputValue value)
		{
			_onConfirmAimInput.Invoke();
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void FlyInput(float newFlyDirection)
		{
			fly = newFlyDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}