using AG.Core;
using AG.Core.Pool;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AG.Gameplay.Actions
{
	public enum ActionStatus
	{
		[LabelText(SdfIconType.StopFill, IconColor = "red")] None,
		[LabelText(SdfIconType.PlayFill, IconColor = "green")] Running,
		[LabelText(SdfIconType.CheckCircle, IconColor = "green")] Finished
	}

	public abstract class BaseAction : SubComponent
	{
		[SerializeField] private ActionId _actionId;
		[SerializeField] private List<FlagSO> _flags;

		// ------------ Private fields -----------------

		private string _typeName;
		private Action _onFinishedCallback;

		private ActionPlayer _actionPlayer;
		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(999)]
		private Animator _animator;

		[ShowInInspector, PropertyOrder(-1)]
		[GUIColor("$" + nameof(StatusColor))]
		private ActionStatus _status;

		private Color StatusColor => _status switch
		{
			ActionStatus.None => Color.white,
			ActionStatus.Running => Color.green,
			ActionStatus.Finished => Color.purple,
			_ => Color.white
		};

		// ----------- Public properties ----------------

		public ActionId ActionId => _actionId;
		public Animator Animator => _animator;
		public List<FlagSO> Flags => _flags;


		public ActionStatus Status
		{
			get => _status;
			protected set => _status = value;
		}

		protected virtual void Awake()
		{
			_typeName = GetType().Name;

			CacheComponents();
		}

		private void CacheComponents()
		{
			_animator ??= Root.Get<Animator>();
		}

		public void StartAction(object parameters, Action onFinished)
		{
			if (Status == ActionStatus.Running)
			{
				Debug.LogError($"Action {_typeName} is already playing");
				onFinished?.Invoke();
				return;
			}

			_onFinishedCallback = onFinished;

			//Debug.Log($"Start {_typeName}");
			Status = ActionStatus.Running;

			DoStartAction(parameters);
		}

		public ActionStatus UpdateAction()
		{
			if (Status != ActionStatus.Running)
			{
				return Status;
			}

			return DoUpdateAction();
		}

		[FoldoutGroup("Debug/Buttons"), PropertyOrder(1001)]
		public void OnActionFinished()
		{
			//Debug.Log($"Action finished: {_typeName}");

			DoOnActionFinished();

			Status = ActionStatus.None;
			_onFinishedCallback?.Invoke();
		}

		public void InterruptAction()
		{
			DoInterruptAction();

			Status = ActionStatus.None;
			_onFinishedCallback?.Invoke();
		}

		protected abstract void DoStartAction(object parameters);
		protected abstract ActionStatus DoUpdateAction();
		protected abstract void DoOnActionFinished();
		protected abstract void DoInterruptAction();

		//------------- DEBUG UTILITIES -----------------------

		[Button("Start", ButtonSizes.Large), PropertyOrder(1000)]
		private void DebugStartAction()
		{
			_actionPlayer.TryPlayAction(_actionId);
		}
	}
}