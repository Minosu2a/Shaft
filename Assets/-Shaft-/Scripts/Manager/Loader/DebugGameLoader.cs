using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameLoader : MonoBehaviour
{


    #region Fields
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private UIManager _uiManager = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private CharacterManager _characterManager = null;
    [SerializeField] private GameStateManager _gameStateManager = null;
    #endregion Fields

    #region Properties
    #endregion Properties

    #region Methods

    private void Start()
    {
        _inputManager.Initialize();
        _gameManager.Initialize();
        _uiManager.Initialize();
        _audioManager.Initialize();
        _characterManager.Initialize();
        _gameStateManager.Initialize();
    }

    #endregion Methods



}
