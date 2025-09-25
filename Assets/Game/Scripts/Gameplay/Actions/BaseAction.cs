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
		[LabelText(SdfIconType.StopFill, IconColor = "white")] None,
		[LabelText(SdfIconType.StopFill, IconColor = "red")] FailedToStart,
		[LabelText(SdfIconType.PlayFill, IconColor = "green")] Running,
		[LabelText(SdfIconType.CheckCircle, IconColor = "purple")] Finished,
		[LabelText(SdfIconType.StopFill, IconColor = "cyan")] Interrupted
	}

	public abstract class BaseAction : SubComponent, IActionStatus
	{
		public event Action<ActionStatus> OnActionFinishedEvent;

		// ------------- Inspector fields -------------
		[SerializeField] private ActionId _actionId;
		[SerializeField] private List<FlagSO> _flags;

		// ------------ Private fields -----------------

		private string _typeName;
		private ActionPlayer _actionPlayer;

		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(999)]
		private Animator _animator;

		[ShowInInspector, PropertyOrder(-1), HideInEditorMode]
		[GUIColor("@" + nameof(StatusColor))]
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
			_actionPlayer ??= Root.Get<ActionPlayer>();
		}

		public void StartAction(object parameters, Action<ActionStatus> onFinished)
		{
			if (Status == ActionStatus.Running)
			{
				Debug.LogError($"Action {_typeName} is already playing");
				onFinished?.Invoke(ActionStatus.FailedToStart);
				return;
			}

			OnActionFinishedEvent = null;
			OnActionFinishedEvent = onFinished;

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
			DoOnActionFinished();

			Status = ActionStatus.Finished;
			OnActionFinishedEvent?.Invoke(Status);
			OnActionFinishedEvent = null;

			Status = ActionStatus.None;
		}

		public void InterruptAction()
		{
			DoInterruptAction();

			Status = ActionStatus.Interrupted;
			OnActionFinishedEvent?.Invoke(Status);
			OnActionFinishedEvent = null;

			Status = ActionStatus.None;
		}

		protected abstract void DoStartAction(object parameters);
		protected virtual ActionStatus DoUpdateAction() { return Status; }
		protected abstract void DoOnActionFinished();
		protected abstract void DoInterruptAction();

		//------------- DEBUG UTILITIES -----------------------

		[Button("Start", ButtonSizes.Large), PropertyOrder(1000)]
		[HideInEditorMode]
		private void DebugStartAction()
		{
			_actionPlayer ??= Root.Get<ActionPlayer>();
			_actionPlayer.TryPlayAction(_actionId);
		}
	}
}