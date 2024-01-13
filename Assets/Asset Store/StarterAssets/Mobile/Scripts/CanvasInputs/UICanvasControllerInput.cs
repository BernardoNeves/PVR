using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        //public StarterAssetsInputs starterAssetsInputs;
        public Inputs _inputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            _inputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            _inputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            _inputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            _inputs.SprintInput(virtualSprintState);
        }
        
    }

}
