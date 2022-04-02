using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    #region Fields
    [Header("Movement")]
    [SerializeField] private Rigidbody _rb = null;
    private bool _isMoving = false;


    [Header("Speeds")]
    [SerializeField] private float _walkSpeed = 250f;
    [SerializeField] private float _airControlForce = 250f;


    [Header("Jump")]
    [SerializeField] private float _jumpForce = 20f;
    [SerializeField] private int _jumpLimit = 1;


    private int _jumpCount = 0;

    [Header("Gravity")]
    private float _groundCheckTimeStamp = 0f;
    private float _jumpingSecurityDelay = 0.3f;
    private float _ySpeed = 0;
    private bool _isGrounded = false;
    private float _distToGround = 0.5f;
    [SerializeField] private float _gravityMultiplier = 2;

    [Header("Rotation")]
    [SerializeField] private GameObject _characterSpriteLeft = null;
    [SerializeField] private GameObject _characterSpriteRight = null;
    [SerializeField] private GameObject _bodyRotation = null;

    #endregion Fields


    #region Properties

    public Rigidbody Rb => _rb;

    public bool IsMoving => _isMoving;



    #endregion Properties


    #region Methods

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CharacterManager.Instance.CharacterController = this;

    }

    private void Update()
    {
        Debug.Log(_rb.velocity);
        //Jump & Gravity
        _ySpeed += (Physics.gravity.y * _gravityMultiplier) * Time.deltaTime;

      



        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Test");
            Jump();
            _jumpCount++;
        }




        GroundCheck();
        Move();

        if (_rb.velocity != Vector3.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }



    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemiLogic")
        {
            Debug.Log("Take Damage");
        }
    }

    private void GroundCheck()
    {
        _groundCheckTimeStamp  += Time.deltaTime;
        if (Physics.Raycast(transform.position, Vector3.down, _distToGround + 0.05f) && _groundCheckTimeStamp >= _jumpingSecurityDelay)
        {
            Debug.Log("Grounded");
            _isGrounded = true;
            _ySpeed = 0f;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void Look()
    {
      //  transform.forward = InputManager.Instance.RotDir;
    }

    public void Move()
    {
        _rb.velocity = new Vector3(InputManager.Instance.MoveDir.x * _walkSpeed, Gravity(), 0);
        if(_rb.velocity.x >= 0.1f)
        {
            _characterSpriteLeft.SetActive(false);
            _characterSpriteRight.SetActive(true);

            _bodyRotation.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if(_rb.velocity.x <= -0.1f)
        {
            _characterSpriteLeft.SetActive(true);
            _characterSpriteRight.SetActive(false);

            _bodyRotation.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

    }

    public float Gravity()
    {
        if(_isGrounded == true)
        {
            return 0;
        }
        else
        {
            return (Physics.gravity.y * _gravityMultiplier) * Time.deltaTime;
        }
        
    }
    public void Jump()
    {
        _groundCheckTimeStamp = 0;
           Vector3 tmpVel = _rb.velocity;
        tmpVel.y = 0;
        _rb.velocity = tmpVel;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }



    #endregion Methods



}
