using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(CharacterManager.Instance.CharacterController.GotCrystal)
            {
                CharacterManager.Instance.CharacterController.Ending();
                Debug.Log("ENDIIIING");
            }
        }
    }
}
