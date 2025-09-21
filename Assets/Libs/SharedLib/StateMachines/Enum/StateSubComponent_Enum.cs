using SharedLib.ComponentCache;
using System;
using UnityEngine;

namespace SharedLib.StateMachines
{
	public abstract class StateSubComponent<T, TStateMachine> : SubComponent, IState<T, TStateMachine>  where T : struct, Enum
	{
		// ------------- Inspector fields -------------
		[SerializeField]
		private T _stateId;


		// ------------- Private fields ----------------
		private TStateMachine _stateMachine;

		// ------------- Public properties -------------
		protected TStateMachine StateMachine => _stateMachine;

		public T StateId => _stateId;
		public void InitState(TStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public abstract void EnterState();
		public abstract void ExitState();
		public abstract IState.Status UpdateState();
	}
}