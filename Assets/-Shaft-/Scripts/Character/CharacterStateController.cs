using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateController : MonoBehaviour
{

    #region Fields
    [SerializeField] private Rigidbody _rb = null;
    [Header("Physic")]
    [SerializeField] private LayerMask _groundLayer = 0;
    [SerializeField] private float _groundThreshold = 1.1f;
    [SerializeField] private float _castRadius = 0.45f;
    [Header("Speeds")]
    [SerializeField] private float _walkSpeed = 250f;
    [SerializeField] private float _sprintSpeed = 500f;
    [SerializeField] private float _airControlForce = 250f;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 40f;
    [SerializeField] private int _jumpLimit = 2;

    private int _jumpCount = 0;
    private bool _isGrounded = false;
    private RaycastHit _hit;

    private ECharacterState _currenStateType = ECharacterState.NONE;
    private Dictionary<ECharacterState, ACharacterState> _states = null;
    #endregion Fields


    #region Properties
    public int JumpLimit => _jumpLimit;

    public int JumpCount
    {
        get => _jumpCount;
        set => _jumpCount = value;
    }
    public Rigidbody Rb => _rb;
    public ACharacterState CurrentState => _states[_currenStateType];

    public bool IsGrounded => _isGrounded;
	#endregion Properties


	#region Methods

    void Start()
    {
        _states = new Dictionary<ECharacterState, ACharacterState>();

        IdleState idleState = new IdleState();
        idleState.Initialize(this, ECharacterState.IDLE);
        _states.Add(ECharacterState.IDLE, idleState);

        WalkState walkState = new WalkState();
        walkState.Initialize(this, ECharacterState.WALK);
        _states.Add(ECharacterState.WALK, walkState);

        JumpState jumpState = new JumpState();
        jumpState.Initialize(this, ECharacterState.JUMP);
        _states.Add(ECharacterState.JUMP, jumpState);

        FallState fallState = new FallState();
        fallState.Initialize(this, ECharacterState.FALL);
        _states.Add(ECharacterState.FALL, fallState);

        SprintState sprintState = new SprintState();
        sprintState.Initialize(this, ECharacterState.SPRINT);
        _states.Add(ECharacterState.SPRINT, sprintState);

        _currenStateType = ECharacterState.IDLE;
    }
    private void Update()
    {
        if(_rb.velocity.x > 0)
        {
            transform.forward = Vector3.right;
        }
        else if (_rb.velocity.x < 0 )
        {
            transform.forward = Vector3.left;
        }
    }
    public void FixedUpdate()
    {
        CurrentState.UpdateState();

        _isGrounded = Physics.SphereCast(transform.position, _castRadius, Vector3.down, out _hit, _groundThreshold, _groundLayer);
    }

    public void ChangeState(ECharacterState newState)
    {
        Debug.Log("Transition from " + _currenStateType + " to " + newState);
        CurrentState.ExitState();
        _currenStateType = newState;
        CurrentState.EnterState();
    }

    public void Walk()
    {
        _rb.velocity = InputManager.Instance.MoveDir * _walkSpeed;
    }

    public void Sprint()
    {
        _rb.velocity = InputManager.Instance.MoveDir * _sprintSpeed;
    }


    public void Jump()
    {
        Vector3 tmpVel = _rb.velocity;
        tmpVel.y = 0;
        _rb.velocity = tmpVel;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    public void AirControl()
    {
        _rb.AddForce(InputManager.Instance.MoveDir * _airControlForce, ForceMode.Force);
        Vector3 tmpVel = _rb.velocity;
        tmpVel.x = Mathf.Clamp(tmpVel.x, -_walkSpeed, _walkSpeed);
        _rb.velocity = tmpVel;
    }
	#endregion Methods


  
}
