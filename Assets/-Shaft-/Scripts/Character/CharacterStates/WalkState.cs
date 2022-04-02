using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : ACharacterState
{
    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods
    public override void EnterState()
    {
        Debug.Log("Enter Walk State");
        InputManager.Instance.OnJumpPressed += Jump;
        InputManager.Instance.OnSprintPressed += Sprint;
        _controller.JumpCount = 0;
    }

    public override void UpdateState()
    {
        _controller.Walk();
        if (InputManager.Instance.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(ECharacterState.IDLE);
        }
    }

    public override void ExitState()
    {
        InputManager.Instance.OnJumpPressed -= Jump;
        InputManager.Instance.OnSprintPressed -= Sprint;
        Debug.Log("Exit Walk State");
    }

    private void Jump()
    {
        _controller.ChangeState(ECharacterState.JUMP);
    }

    private void Sprint()
    {
        _controller.ChangeState(ECharacterState.SPRINT);
    }
    #endregion Methods
}
