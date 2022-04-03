using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private Collider _playerDetection = null;
    private float _trapTimeStamp = 1.4f;
    [SerializeField] private float _damageTimerApply = 1.5f;
    private bool _trapDamageGate = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerDetection = other;
        }
    }

    private void Update()
    {
        if(_playerDetection != null)
        {
            _trapTimeStamp += Time.fixedDeltaTime;
         
            if(_trapTimeStamp >= _damageTimerApply)
            {
                _trapTimeStamp = 0f;
                CharacterManager.Instance.CharacterController.TakeDamage(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerDetection = null;
            _trapTimeStamp = 1.4f;
        }
    }



}
