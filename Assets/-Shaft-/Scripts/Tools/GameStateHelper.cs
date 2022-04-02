using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateHelper
{
    public static string SetSceneName(EGameState state)
    {
        string sceneName = string.Empty;
        switch (state)
        {
            case EGameState.INITIALIZE:
                sceneName = "PersistantManager";
                    break;
            case EGameState.MAINMENU:
                sceneName = "MainMenu";
                break;
            case EGameState.GAME:
                sceneName = "Game";
                break;


        }
        return sceneName;
    }

}
