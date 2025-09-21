using AG.Core.Pool;
using SharedLib.ComponentCache;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class ActionPlayer : SubComponent, IPooledComponent
	{

		//--------------------------------

		private Animator _animator;
		private Flags _flags;

		//--------------------------------

		public Animator Animator => _animator;

		//--------------------------------

		private List<BaseAction> _runningActions = new();
		private List<BaseAction> _finishedActions = new();

		private Dictionary<ActionId, BaseAction> _actionMap = new();

		protected void Awake()
		{
			_animator = Root.Get<Animator>();
			_flags = Root.Get<Flags>();

			foreach (BaseAction action in Root.GetAll<BaseAction>())
			{
				_actionMap[action.ActionId] = action;
			}
		}

		void Update()
		{
			foreach (BaseAction action in _runningActions)
			{
				if (action.UpdateAction() == ActionStatus.Finished)
				{
					_finishedActions.Add(action);
				}
			}

			foreach (BaseAction action in _finishedActions)
			{
				ProcessActionFinished(action);
			}

			_runningActions.RemoveAll(action => _finishedActions.Contains(action));
			_finishedActions.Clear();
		}

		public bool TryPlayAction(ActionId actionId, object parameters = null, Action onFinished = null)
		{
			if (!_actionMap.TryGetValue(actionId, out BaseAction action))
			{
				Debug.LogError($"Action {actionId} not found");
				return false;
			}

			if (!CheckCanPlayAction(action))
			{
				return false;
			}

			action.StartAction(parameters, onFinished);

			_flags.RaiseFlags(action.Flags);

			_runningActions.Add(action);

			return false;
		}

		private void ProcessActionFinished(BaseAction action)
		{
			if (action.Status != ActionStatus.Finished)
			{
				Debug.LogError($"Action {action.GetType().Name} is not finished");
				return;
			}

			action.OnActionFinished();

			_flags.LowerFlags(action.Flags);
		}

		private bool CheckCanPlayAction(BaseAction action)
		{
			if (action.Status == ActionStatus.Running)
			{
				Debug.LogError($"Action {action.GetType().Name} is already playing");
				return false;
			}

			if (action.Status == ActionStatus.Finished)
			{
				Debug.LogError($"Action {action.GetType().Name} is already finished");
				return false;
			}

			return true;
		}

		public void OnBeforeGetFromPool()
		{
		}

		public void OnAfterGetFromPool()
		{
		}

		public void OnReturnToPool()
		{
			foreach (BaseAction action in _runningActions)
			{
				action.InterruptAction();
			}
		}

		public void OnDestroyFromPool()
		{
		}
	}
}