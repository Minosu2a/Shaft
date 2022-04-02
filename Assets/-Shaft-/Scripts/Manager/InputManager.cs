using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{


    #region Fields
    private Vector3 _moveDir = Vector3.zero;
    public bool _sprintEnabled = false;


    #endregion Fields

    #region Properties
    public Vector3 MoveDir => _moveDir;
    public bool SprintEnabled => _sprintEnabled;
    #endregion Properties

    #region Events
    private event Action _onJumpPressed = null;
    public event Action OnJumpPressed
    {
        add
        {
            _onJumpPressed -= value;
            _onJumpPressed += value;
        }
        remove
        {
            _onJumpPressed -= value;
        }
    }

    private event Action _onSprintPressed = null;
    public event Action OnSprintPressed
    {
        add
        {
            _onSprintPressed -= value;
            _onSprintPressed += value;
        }
        remove
        {
            _onSprintPressed -= value;
        }
    }
    #endregion Events

    #region Methods
    public void Initialize()
    {

    }


    protected override void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(_onJumpPressed != null)
                _onJumpPressed();
        }

        if(Input.GetButtonDown("Fire3"))
        {
            if (_onSprintPressed != null)
            {
                _onSprintPressed();
                _sprintEnabled = true;
            }
        }

        if(Input.GetButtonUp("Fire3"))
        {
            _sprintEnabled = false;
        }

        _moveDir.x = Input.GetAxis("Horizontal");
       // _moveDir.y = Input.GetAxis("Vertical");
    }

    #endregion Methods



}
