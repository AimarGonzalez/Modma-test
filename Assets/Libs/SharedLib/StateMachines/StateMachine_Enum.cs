using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharedLib.StateMachines
{
	public class StateMachine<TId> where TId : struct, Enum
	{
		// ------------- Events -------------
		public event Action<TId, TId> OnStateTransition;
		public event Action<TId> OnStateFinishedWithoutTransition;

		// ------------- Private fields -------------
		private IState<TId> _currentState;
		private IState<TId> _previousState;
		private TId _currentStateId;
		private TId _previousStateId;

		// ------------- Public properties -------------
		public IState<TId> CurrentState => _currentState;
		public TId CurrentStateId => _currentStateId;
		public TId PreviousStateId => _previousStateId;

		private Dictionary<TId, IState<TId>> _statesMap = new();

		public void AddState(IState<TId> state)
		{
			_statesMap[state.StateId] = state;

			state.InitState(this);
		}

		public void RemoveState(TId TStateId)
		{
			_statesMap.Remove(TStateId);
		}

		public void SetState(TId nextStateId)
		{
			if (!_statesMap.TryGetValue(nextStateId, out IState<TId> nextState))
			{
				Debug.LogError($"State {nextStateId} not found");
				return;
			}

			SetState(nextState);
		}

		public void SetState(IState<TId> nextState)
		{
			_previousState = _currentState;
			_previousStateId = _currentStateId;

			_previousState?.ExitState();

			_currentState = nextState;
			_currentStateId = nextState.StateId;

			_currentState.EnterState();

			OnStateTransition?.Invoke(_previousStateId, _currentStateId);
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
				OnStateFinishedWithoutTransition?.Invoke(_currentStateId);
			}
		}

	}
}