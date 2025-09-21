using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
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

		[ShowInInspector, HideInEditorMode, GUIColor("@" + nameof(ColorOfCurrentState)), PropertyOrder(-100)]
		protected bool IsCurrentState => _stateMachine != null && _stateMachine.CurrentStateId == _stateId;
		private Color ColorOfCurrentState => IsCurrentState ? Color.green : Color.white;

		StateId IState.StateId => _stateId;

		void IState.InitState(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public abstract void OnEnterState();
		public abstract void OnExitState();
		public abstract IState.Status UpdateState();
	}
}

