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
    private bool _isLightOn = true;
    private float _lightOffTimeStamp = 0f;
    private float _currentFuel = 240;
    [Space]
    [SerializeField] private float _maxFuel = 240;
    private float _lightRemainingPerc;
    private bool _outOfFuel = false;

    [Header("Maps")]
    [SerializeField] private MapController _mapController = null;

    [Header("Damage")]
    [SerializeField] private int _maxHp = 10;
    private int _currentHp;
    [Space]
    [SerializeField] private float _xBlockDamageThresold = 0.2f;
    [SerializeField] private float _yBlockDamageThresold = 0.25f;
    private bool _death = false;
    [Header("Camera")]
    [SerializeField] private Camera _camera = null;
    private float _defaultCamFOV = 60;
    [SerializeField] private float _damageCamFOV = 45;
    private bool _camAnimation = false;
    [SerializeField] private float _damageZoomDelay = 0.1f;
    [SerializeField] private float _resetZoomResetDelay = 0.4f;

    private float _timerForDamageZoom = 0.2f;
     private float _timerForZoomReset = 0.4f;

    

    [Header("Footsteps")]
    [SerializeField] private float _footstepTimer = 0.4f;
    [SerializeField] private Transform _footPosition = null;
    [SerializeField] private float _stopFootstepTimestampDivider = 1.35f;
    private float _footstepTimeStamp = 0f;
    [Space]
    [SerializeField] private float _velocityToTakeDamage = -11f;
    private bool _fallingCondition = false;
    private bool _damageOnFall = false;
    private float _fallTimeStampSecurity = 0f;

    [Header("Monster")]
    [SerializeField] private float _startMonsterTime = 8f;
    private bool _monsterComing = false;
    private bool _monsterGate = false;
    [SerializeField] private Transform _monsterPos = null;
    private Coroutine _monsterCoroutine = null;
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
                try
                {
                    CancelMonster();
                }
                catch
                {
                    Debug.Log("NoCouroutineFound I guess");
                }
                AudioManager.Instance.Start3DSound("S_Damage", transform);
                _death = true;
                _camAnimation = true;
            }
            else
            {
                AudioManager.Instance.Start3DSound("S_Damage", transform);
                _camAnimation = true;
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
        _currentFuel = _maxFuel;

        _monsterCoroutine = StartCoroutine(MonsterKill());
        _defaultCamFOV = _camera.fieldOfView;

        _timerForDamageZoom = _damageZoomDelay;
        _timerForZoomReset = _resetZoomResetDelay;
    }

    IEnumerator MonsterKill()
    {
        AudioManager.Instance.StartMonsterApproach();

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.StartTeasingSound();
        float randomX;
        float randomY;



        yield return new WaitForSeconds(2f);

        TakeDamage(1);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position = new Vector3(randomX, randomY, transform.position.z);        
        switch(Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
        
        yield return new WaitForSeconds(1f);

        TakeDamage(1);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position =  new Vector3(randomX, randomY, transform.position.z);
        switch (Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
        
        yield return new WaitForSeconds(1f);

        TakeDamage(2);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position = new Vector3(randomX, randomY, transform.position.z);
        switch (Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
        
        yield return new WaitForSeconds(1f);

        TakeDamage(2);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position = new Vector3(randomX, randomY, transform.position.z);
        switch (Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
        
        yield return new WaitForSeconds(1f);

        TakeDamage(2);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position = new Vector3(randomX, randomY, transform.position.z);
        switch (Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
        
        yield return new WaitForSeconds(1f);

        TakeDamage(2);
        randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        randomY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
        _monsterPos.position = new Vector3(randomX, randomY, transform.position.z);
        switch (Random.Range(0, 1)) { case 0: AudioManager.Instance.Start3DSound("S_MonsterDamage1", _monsterPos); break; case 1: AudioManager.Instance.Start3DSound("S_MonsterDamage2", _monsterPos); break; }
    }

    private void CancelMonster()
    {
        Debug.Log("ça marche ?");
        StopCoroutine(_monsterCoroutine);
        AudioManager.Instance.StartMonsterApproach(false);
        AudioManager.Instance.StartTeasingSound(false);
        // AudioManager.Instance.StopMonsterSound();
        _monsterGate = false;
    }

    private void Update()
    {
        if(_camAnimation == true)
        {
            Debug.Log("SALUT " + _timerForDamageZoom);
            if(_timerForDamageZoom >= 0)
            {
                Debug.Log("Zoom");
                _timerForDamageZoom -= Time.fixedDeltaTime;
                float zoomTime = Mathf.InverseLerp(_damageZoomDelay, 0f, _timerForDamageZoom);
                _camera.fieldOfView = Mathf.Lerp(_defaultCamFOV, _damageCamFOV, zoomTime);
            }
            else if(_timerForZoomReset >= 0)
            {
                Debug.Log("Reset");
                _timerForZoomReset -= Time.fixedDeltaTime;
                float resetZoomTime = Mathf.InverseLerp(_resetZoomResetDelay, 0f, _timerForZoomReset);
                _camera.fieldOfView = Mathf.Lerp(_damageCamFOV,  _defaultCamFOV, resetZoomTime);
            }
            else
            {
                _camAnimation = false;
                _timerForDamageZoom = _damageZoomDelay;
                _timerForZoomReset = _resetZoomResetDelay;
            }


        }


        if(_death != true)
        {

            if (_isLightOn == false)
            {
                _lightOffTimeStamp += Time.fixedDeltaTime;

                if (_lightOffTimeStamp >= _startMonsterTime && _monsterGate == false)
                {
                    _monsterComing = true;
                    _monsterCoroutine = StartCoroutine(MonsterKill());
                    _monsterGate = true;
                    // Suicide Activation
                }

            

            }
            else
            {
                if(_monsterComing == true)
                {
                    CancelMonster();
                }

                _lightOffTimeStamp = 0f;
                _monsterComing = false;
                _currentFuel -= Time.fixedDeltaTime;

                if (_currentFuel <= _maxFuel / 3.5f)
                {

                    _lightRemainingPerc = Mathf.InverseLerp(_maxFuel / 3.5f, 0f, _currentFuel);

                    _pointLight.intensity = Mathf.Lerp(_defaultPointLightIntensity, 0, _lightRemainingPerc);
                    _beamLight.intensity = Mathf.Lerp(_defaultBeamLightIntensity, 0, _lightRemainingPerc);

                }
                else if (_currentFuel <= _maxFuel / 2)
                {
                    //Flicker Activation
                }

                if (_currentFuel <= 0)
                {
                    LightOff();
                    _outOfFuel = true;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (_isLightOn == true)
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
                if (GroundCheck() == true)
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
                _footstepTimeStamp = _footstepTimer / _stopFootstepTimestampDivider;
            }

            if ((_rb.velocity.x >= 0.1f || _rb.velocity.x <= -0.1f) && GroundCheck() == true)
            {
                _footstepTimeStamp += Time.deltaTime;

                if (_footstepTimeStamp >= _footstepTimer && GroundCheck() == true)
                {
                    Footstep();
                }
            }

            _fallTimeStampSecurity += Time.deltaTime;

            if ((_fallingCondition == true && GroundCheck() == true) && _fallTimeStampSecurity >= 0.8f)
            {
                _fallTimeStampSecurity = 0f;
                _fallingCondition = false;

                Debug.Log("FALL");
                AudioManager.Instance.Start3DSound("S_Fall", _footPosition);
                if (_damageOnFall)
                {
                    TakeDamage(1);
                }
                _damageOnFall = false;

            }


            if ((_rb.velocity.y <= -5f && _fallingCondition == false) && _fallTimeStampSecurity >= 0.8f)
            {
                Debug.Log("FallingCondition");
                _fallingCondition = true;
            }

            if (_rb.velocity.y <= _velocityToTakeDamage && _fallingCondition == true)
            {
                Debug.Log("ShouldTakeDamage");
                _damageOnFall = true;
            }
        }

      

    }

    public void TakeDamage(int hp)
    {
        CurrentHp = CurrentHp - hp;
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

    private void Footstep()
    {
        _footstepTimeStamp = 0f;
      int rand =  Random.Range(0, 4);
        switch(rand)
        {
            case 0:
                AudioManager.Instance.Start3DSound("S_Footstep1",_footPosition);
                break;
            case 1:
                AudioManager.Instance.Start3DSound("S_Footstep2", _footPosition);
                break;
            case 2:
                AudioManager.Instance.Start3DSound("S_Footstep3", _footPosition);
                break;
            case 3:
                AudioManager.Instance.Start3DSound("S_Footstep4", _footPosition);
                break;
        }
    }

   

    private void LightOn()
    {
        if(_outOfFuel == false)
        {
            _isLightOn = true;
            _pointLight.intensity = _defaultPointLightIntensity;
            _beamLight.intensity = _defaultBeamLightIntensity;
        }
        //Feedback
        AudioManager.Instance.Start3DSound("S_LightOn", transform);

    }



    private void LightOff()
    {
        //Feedback
        _isLightOn = false;
        _pointLight.intensity = 0;
        _beamLight.intensity = 0;
        _mapController.MapSwitch();
        _lightOffTimeStamp = 0f;
        AudioManager.Instance.Start3DSound("S_LightOff", transform);
    }



    #region Movement 
    private bool GroundCheck()
    {

        
        if (Physics.Raycast(GetComponent<CapsuleCollider>().transform.position, Vector3.down, _distToGround + 1f))
        {
           // Debug.Log("Grounded");
            return true;
        }
        else
        {
           // Debug.Log("Not Grounded");

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
        AudioManager.Instance.Start3DSound("S_Jump", transform);
        _rb.velocity = Vector3.up * _jumpForce;
      //  _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    #endregion Movement 



    #endregion Methods



}
