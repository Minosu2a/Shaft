using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{

    #region Fields

    private bool _isPaused = false;
    #endregion Fields


    #region Property
    private bool IsPaused
    {
        get
        {
            return _isPaused;
        }
        set
        {
            _isPaused = value;
            _onPause(_isPaused);
        }
    }
    #endregion Property


    #region Events
    private event Action<bool> _onPause = null;
    public event Action<bool> OnPause
    {
        add
        {
            _onPause -= value;
            _onPause += value;

        }
        remove
        {
            _onPause -= value;

        }
    }
    #endregion Events


    #region Methods

    public void Initialize()
    {

    }


    public void LoadScene(string sceneName)
    {
       SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        IsPaused = true;
    }

    public void UnpauseGame()
    {
        IsPaused = false;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
    }
    #endregion Methods
}
