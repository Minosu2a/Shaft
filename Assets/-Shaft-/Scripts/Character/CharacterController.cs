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


    [Header("Light")]
    [SerializeField] private Light _pointLight = null;
    [SerializeField] private Light _beamLight = null;
    [SerializeField] private float _timeToTurnOffLight = 1f;
    private float _defaultPointLightIntensity = 1f;
    private float _defaultBeamLightIntensity = 1f;
    private bool _isLightOn = false;

    [Header("Maps")]
    [SerializeField] private MapController _mapController = null;

    [Space]
    [Header("Damage")]
    [SerializeField] private int _maxHp = 10;
    private int _currentHp;
    [SerializeField] private float _xBlockDamageThresold = 0.2f;
    [SerializeField] private float _yBlockDamageThresold = 0.25f;
    #endregion Fields


    #region Properties

    public Rigidbody Rb => _rb;

    public bool IsMoving => _isMoving;

    public int CurrentHp
    {
        get
        {
            return _currentHp;
        }
        set
        {
            _currentHp = value;
            if(_currentHp <= 0)
            {
                Debug.Log("DEAAATH !");
            }
        }
    }

    #endregion Properties


    #region Methods

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CharacterManager.Instance.CharacterController = this;

        _defaultPointLightIntensity = _pointLight.intensity;
        _defaultBeamLightIntensity = _beamLight.intensity;

        _currentHp = _maxHp;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            if(_isLightOn == true)
            {
                LightOff();
            }
            else
            {
                LightOn();
            }
        }


        if (Input.GetButtonDown("Jump"))
        {
            if(GroundCheck() == true)
            {
                Jump();
                _jumpCount++;
            }
        }



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


        float xDifference = Mathf.Abs(other.transform.position.x - transform.position.x);
        float yDifference = Mathf.Abs(other.transform.position.y - transform.position.y);

        //Debug.Log(xDifference + "  " + yDifference);

        if ((xDifference <= _xBlockDamageThresold) && (yDifference <= _yBlockDamageThresold))
        {
            Debug.Log("Damage / Current HP " + _currentHp);
            CurrentHp--;
        }

        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("Take Damage");
        }
    }



    private void LightOn()
    {
        _isLightOn = true;
        _pointLight.intensity = _defaultPointLightIntensity;
        _beamLight.intensity = _defaultBeamLightIntensity;
        //Feedback
    }

    private void LightOff()
    {
        //Feedback
        _isLightOn = false;
        _pointLight.intensity = 0;
        _beamLight.intensity = 0;
        _mapController.MapSwitch();
    }



    #region Movement 
    private bool GroundCheck()
    {

        
        if (Physics.Raycast(GetComponent<CapsuleCollider>().transform.position, Vector3.down, _distToGround + 1f))
        {
            Debug.Log("Grounded");
            return true;
        }
        else
        {
            Debug.Log("Not Grounded");

            _isGrounded = false;
            return false;
        }
    }

    private void Look()
    {
      //  transform.forward = InputManager.Instance.RotDir;
    }

    public void Move()
    {
        _rb.velocity = new Vector3(InputManager.Instance.MoveDir.x * _walkSpeed, _rb.velocity.y, 0);

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
        // _groundCheckTimeStamp = 0;
        //   Vector3 tmpVel = _rb.velocity;
        //    tmpVel.y = 0;
        // _rb.velocity = tmpVel;
        _rb.velocity = Vector3.up * _jumpForce;
      //  _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    #endregion Movement 



    #endregion Methods



}
