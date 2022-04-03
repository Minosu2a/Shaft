using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Animator _fade = null;
    #endregion Fields
    #region Property
    #endregion Property


    #region Methods
    private void Awake()
    {
        UIManager.Instance.UIController = this;    
    }
    private void Start()
    {
        _fade.SetTrigger("FadeIn");
    }

    public void TooglePause()
    {
        GameManager.Instance.TogglePause();
    }
    #endregion Methods


}
