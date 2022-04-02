using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{


    #region Fields

    private EGameState _currenStateType = EGameState.NONE;
    private Dictionary<EGameState, AGameState> _states = null;

    private EGameState _nextState = EGameState.NONE;
    private EGameState _previousState = EGameState.NONE;


    #endregion Fields

    #region Properties
    public AGameState CurrentState => _states[_currenStateType];

    public EGameState NextState => _nextState;
    public EGameState PreviousState => _previousState;


    #endregion Properties

    #region Methods
    public void Initialize()
    {
        _states = new Dictionary<EGameState, AGameState>();

        InitializeState initializeState = new InitializeState();
        initializeState.Initialize(EGameState.INITIALIZE);
        _states.Add(EGameState.INITIALIZE, initializeState);

        LoadingState loadingState = new LoadingState();
        loadingState.Initialize(EGameState.LOADING);
        _states.Add(EGameState.LOADING, loadingState);

        MainMenuState mainMenuState = new MainMenuState();
        mainMenuState.Initialize(EGameState.MAINMENU);
        _states.Add(EGameState.MAINMENU, mainMenuState);

        GameState gameState = new GameState();
        gameState.Initialize(EGameState.GAME);
        _states.Add(EGameState.GAME, gameState);

        _currenStateType = EGameState.INITIALIZE;
    }

    protected override void Update()
    {
        CurrentState.UpdateState();
    }

    public void ChangeState(EGameState newState)
    {
        Debug.Log("Transition from " + _currenStateType + " to : " + newState);

        CurrentState.ExitState();
        _previousState = _currenStateType;

        _currenStateType = newState;

        CurrentState.EnterState();
    }

    public void LaunchTransition(EGameState newState)
    {
        _previousState = _currenStateType;
        _nextState = newState;
        ChangeState(EGameState.LOADING);

    }

    #endregion Methods



}
