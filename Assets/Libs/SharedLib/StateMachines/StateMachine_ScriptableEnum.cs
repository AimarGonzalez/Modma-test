using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharedLib.StateMachines
{
	public class StateMachine
	{
		// ------------- Events -------------
		public event Action<StateId, StateId> OnStateTransition;
		public event Action<StateId> OnStateFinishedWithoutTransition;

		// ------------- Private fields -------------
		private IState _currentState;
		private IState _previousState;

		// ------------- Public properties -------------
		public IState CurrentState => _currentState;
		public StateId CurrentStateId => _currentState?.StateId;
		public StateId PreviousStateId => _previousState?.StateId;

		private Dictionary<StateId, IState> _statesMap = new();

		public void AddState(IState state)
		{
			_statesMap[state.StateId] = state;

			state.InitState(this);
		}

		public void RemoveState(StateId stateId)
		{
			_statesMap.Remove(stateId);
		}

		public void SetState(StateId stateId)
		{
			if (!_statesMap.TryGetValue(stateId, out IState nextState))
			{
				Debug.LogError($"State {stateId} not found");
				return;
			}

			SetState(nextState);
		}

		public void SetState(IState nextState)
		{
			_previousState = _currentState;

			_previousState?.ExitState();
			_currentState = nextState;
			_currentState.EnterState();

			OnStateTransition?.Invoke(_previousState?.StateId, nextState.StateId);
		}

		private void Update()
		{
			if (_currentState == null)
			{
				return;
			}

			IState.Status status = _currentState.UpdateState();

			if (status == IState.Status.Finished)
			{
				OnStateFinishedWithoutTransition?.Invoke(_currentState?.StateId);
			}
		}
	}
}