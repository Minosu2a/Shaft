using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACharacterState
{


	#region Fields
	protected CharacterStateController _controller = null;
	protected ECharacterState _state = ECharacterState.NONE;
	#endregion Fields

	#region Properties
	#endregion Properties

	#region Methods
	public void Initialize(CharacterStateController controller, ECharacterState state)
	{
		_controller = controller;
		_state = state;
    }

	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void ExitState();

	#endregion Methods


  
}
