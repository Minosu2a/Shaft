using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ACharacterState
{
    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    public override void EnterState()
    {
        Debug.Log("Enter Idle State");
        InputManager.Instance.OnJumpPressed += Jump;
        _controller.JumpCount = 0;
    }

    public override void UpdateState()
    {
        if(InputManager.Instance.MoveDir != Vector3.zero)
        {
            _controller.ChangeState(ECharacterState.WALK);
        }
    }

    public override void ExitState()
    {
        InputManager.Instance.OnJumpPressed -= Jump;
        Debug.Log("Exit Idle State");
    }

    private void Jump()
    {
        _controller.ChangeState(ECharacterState.JUMP);
    }
    #endregion Methods

}
