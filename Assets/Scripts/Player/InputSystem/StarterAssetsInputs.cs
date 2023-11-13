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