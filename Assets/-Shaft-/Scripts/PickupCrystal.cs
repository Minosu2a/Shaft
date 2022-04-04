using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCrystal : MonoBehaviour
{

    private Collider _playerDetection = null;
    [SerializeField] private GameObject _sprite = null;
    [SerializeField] private GameObject _particleRed = null;
    [SerializeField] private GameObject _particleBlue = null;
    [SerializeField] private float _fuelGiven = 80f;
    private bool _pickedUp = false;
    [SerializeField] private bool _lifeCrystal = false;
    [SerializeField] private GameObject _lifeCrystalObject = null;

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

        Instantiate(_particleRed, transform.position, Quaternion.identity);

        CharacterManager.Instance.CharacterController.CurrentFuel += _fuelGiven;

        if(_lifeCrystal == true)
        {
            AudioManager.Instance.Start3DSound("S_Crystal", transform);
            Debug.Log("Crystal Pick Up Extra Feedback");
            CharacterManager.Instance.CharacterController.GotCrystal = true;
            Instantiate(_particleBlue, transform.position, Quaternion.identity);

            if (CharacterManager.Instance.CharacterController.FirstTimePickingTheCrystal == false)
            {
                AudioManager.Instance.PlayMusicWithFadeIn("M_Delusions", 1.5f);
                CharacterManager.Instance.CharacterController.FirstTimePickingTheCrystal = true;
            }
        }

    

    }

    public void LifeCrystalInit()
    {
        _lifeCrystal = true;
        _lifeCrystalObject.SetActive(true);
        //Some Light and sound

      
    }

}
