using SharedLib.ComponentCache;
using UnityEngine;

namespace SharedLib.StateMachines
{
	public abstract class StateSubComponent : SubComponent, IState
	{

		// ------------- Inspector fields -------------
		[SerializeField]
		private StateId _stateId;


		// ------------- Private fields ----------------
		private StateMachine _stateMachine;

		// ------------- Public properties -------------
		protected StateMachine StateMachine => _stateMachine;

		StateId IState.StateId => _stateId;
		void IState.InitState(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public abstract void EnterState();
		public abstract void ExitState();
		public abstract IState.Status UpdateState();
	}
}