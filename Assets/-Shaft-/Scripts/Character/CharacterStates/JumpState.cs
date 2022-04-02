using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : ACharacterState
{
    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    public override void EnterState()
    {
        Debug.Log("Enter Jump State");
        _controller.Jump();
        _controller.JumpCount++;
        InputManager.Instance.OnJumpPressed += Jump;

    }

    public override void UpdateState()
    {
        _controller.AirControl();
        if (_controller.Rb.velocity.y < 0)
        {
            _controller.ChangeState(ECharacterState.FALL);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit Jump State");
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
