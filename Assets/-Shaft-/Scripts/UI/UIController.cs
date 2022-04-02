using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Fields
    #endregion Fields
    #region Property
    #endregion Property


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }

    public void TooglePause()
    {
        GameManager.Instance.TogglePause();
    }
    #endregion Methods


}
