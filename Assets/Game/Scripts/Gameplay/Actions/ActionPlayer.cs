using AG.Core.Pool;
using AG.Core.UI;
using SharedLib.ComponentCache;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class ActionPlayer : SubComponent, IPooledComponent, IDebugPanelDrawer
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

		public IActionStatus TryPlayAction(ActionId actionId, Action<ActionStatus> onFinished = null)
		{
			return PlayAction(actionId, null, onFinished, true);
		}

		public IActionStatus PlayAction(ActionId actionId, Action<ActionStatus> onFinished = null, bool failSilently = false)
		{
			return PlayAction(actionId, null, onFinished);
		}

		public IActionStatus PlayAction(ActionId actionId, object parameters, Action<ActionStatus> onFinished = null, bool failSilently = false)
		{
			if (!_actionMap.TryGetValue(actionId, out BaseAction action))
			{
				Debug.LogError($"Action {actionId} not found");
				onFinished?.Invoke(ActionStatus.FailedToStart);
				return null;
			}

			if (!CheckCanPlayAction(action, failSilently))
			{
				return null;
			}

			action.StartAction(parameters, onFinished);

			_flags.RaiseFlags(action.Flags);

			_runningActions.Add(action);

			return action;
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

		private bool CheckCanPlayAction(BaseAction action, bool failSilently)
		{
			if (action.Status == ActionStatus.Running)
			{
				if (!failSilently)
				{
					Debug.LogError($"Action {action.GetType().Name} is already playing");
				}
				return false;
			}

			if (action.Status == ActionStatus.Finished)
			{
				if (!failSilently)
				{
					Debug.LogError($"Action {action.GetType().Name} is already finished");
				}
				return false;
			}

			return true;
		}

		public void StopAction(IActionStatus actionStatus)
		{
			if (actionStatus is BaseAction action)
			{
				action.InterruptAction();
				_runningActions.Remove(action);
			}
			else
			{
				Debug.LogError($"Action {actionStatus.GetType().Name} is not a BaseAction");
			}
		}

		public void StopActions(ActionId actionId)
		{
			bool found = false;
			for (int i = _runningActions.Count - 1; i >= 0; i--)
			{
				BaseAction action = _runningActions[i];
				if (action.ActionId == actionId)
				{
					action.InterruptAction();
					_runningActions.RemoveAt(i);
					found = true;
				}
			}

			if (!found)
			{
				Debug.LogError($"Action {actionId} not found");
			}
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

		public void AddDebugProperties(List<GUIUtils.Property> properties)
		{
			properties.Add(new GUIUtils.Property("Running Actions", _runningActions.Count));
			foreach (BaseAction action in _runningActions)
			{
				properties.Add(new GUIUtils.Property($"  {action.ActionId}", action.Status));
			}
		}
	}
}