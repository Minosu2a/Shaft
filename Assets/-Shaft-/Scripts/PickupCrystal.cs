using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCrystal : MonoBehaviour
{

    private Collider _playerDetection = null;
    [SerializeField] private GameObject _sprite = null;
    [SerializeField] private GameObject _particle = null;
    [SerializeField] private float _fuelGiven = 80f;
    private bool _pickedUp = false;

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
            if (Input.GetButtonDown("Fire2") && _pickedUp == false) 
            {
                Debug.Log("Object Taken");
                Pickup();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerDetection = null;
        }
    }

 
    private void Pickup()
    {
        AudioManager.Instance.Start3DSound("S_Gather", transform);
        _pickedUp = true;
        _sprite.SetActive(false);
        Instantiate(_particle,transform.position,Quaternion.identity);
    }

}
