using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : ACharacterState
{

    #region Fields
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods

    public override void EnterState()
    {
        Debug.Log("Enter Sprint State");
        InputManager.Instance.OnJumpPressed += Jump;

        _controller.JumpCount = 0;

    }

    public override void UpdateState()
    {
        _controller.Sprint();

        if (InputManager.Instance.MoveDir == Vector3.zero) //Issue is when we spam right -> left -> right again and again
        {
            _controller.ChangeState(ECharacterState.IDLE);
        }
        else if (InputManager.Instance.SprintEnabled == false && InputManager.Instance.MoveDir != Vector3.zero)
        {
            _controller.ChangeState(ECharacterState.WALK);
        }


    }

    public override void ExitState()
    {
        InputManager.Instance.OnJumpPressed -= Jump;
        Debug.Log("Exit Sprint State");
    }

    private void Jump()
    {
        _controller.ChangeState(ECharacterState.JUMP);
    }

    #endregion Methods



}
