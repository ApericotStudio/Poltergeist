using UnityEngine;
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

		private AimMode _aimMode;
		private PossessionController _possessionController;

        private void Awake()
        {
			_aimMode = gameObject.GetComponent<AimMode>();
			_possessionController = gameObject.GetComponent<PossessionController>();
        }

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
			if(cursorInputForLook)
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
			_aimMode.EnterAimMode();
        }

		private void OnAimCancel(InputValue value)
        {
			if (_possessionController.CurrentPossession != null && !_aimMode.aimmode)
            {
				_possessionController.Unpossess();
            } else
            {
				_aimMode.ExitAimMode();
			}

        }

		private void OnAimConfirm(InputValue value)
        {
			if (_possessionController.CurrentPossession == null)
			{
				_possessionController.Possess();
			}
			else
			{
				if (_possessionController.currentThrowable != null && _aimMode.aimmode)
				{
					_possessionController.currentThrowable.Throw();
				}
			}
			_aimMode.ExitAimMode();
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