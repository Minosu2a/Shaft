using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : ACharacterState
{
    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    public override void EnterState()
    {
        Debug.Log("Enter Fall State");
        InputManager.Instance.OnJumpPressed += Jump;
    }

    public override void UpdateState()
    {


        _controller.AirControl();
        if(_controller.IsGrounded)
        {
            _controller.ChangeState(ECharacterState.WALK);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit Fall State");
        InputManager.Instance.OnJumpPressed -= Jump;

    }

    private void Jump()
    {
        if (_controller.JumpCount <= _controller.JumpLimit)
        {
            _controller.ChangeState(ECharacterState.JUMP);
        }
    }
    #endregion Methods
}
