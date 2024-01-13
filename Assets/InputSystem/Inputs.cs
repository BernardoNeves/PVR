using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

//namespace StarterAssets
//{
	public class Inputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool shoot;
		public bool reload;
		public bool interact;
		public float weaponSwap;
		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

//#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{

			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
        {
			InteractInput(value.isPressed);
        }
		
		public void OnShoot(InputValue value)
        {
			ShootInput(value.isPressed);
        }
		public void OnReload(InputValue value)
        {
			ReloadInput(value.isPressed);
        }

		public void OnWeaponSwap(InputValue value)
        {
			WeaponSwapInput(value.Get<float>());
        }

//#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
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

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
		
		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
		}

		public void WeaponSwapInput(float newWeaponSwapState)
		{
			weaponSwap = newWeaponSwapState;
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
	
//}