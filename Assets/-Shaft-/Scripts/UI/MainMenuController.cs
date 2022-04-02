using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    
    #region Fields
	#endregion Fields

	#region Properties
	#endregion Properties

	#region Methods

    public void Play()
    {
        GameStateManager.Instance.LaunchTransition(EGameState.GAME);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion Methods



}
