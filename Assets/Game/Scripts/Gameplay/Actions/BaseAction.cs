using AG.Core;
using AG.Core.Pool;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
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
		[SerializeField] private List<FlagSO> _flags;

		//--------------------------------

		private string _typeName;

		private ActionPlayer _actionPlayer;
		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(999)]
		private Animator _animator;
		
		[ShowInInspector, PropertyOrder(-1)]
		[GUIColor("@_status==ActionStatus.Running?\"green\" : \"white\"")]
		private ActionStatus _status;
		
		//--------------------------------
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

		public void StartAction(object parameters)
		{
			if (Status == ActionStatus.Running)
			{
				Debug.LogError($"Action {_typeName} is already playing");
				return;
			}

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
		}

		public void InterruptAction()
		{
			DoInterruptAction();
			
			Status = ActionStatus.None;
		}

		protected abstract void DoStartAction(object parameters);
		protected abstract ActionStatus DoUpdateAction();
		protected abstract void DoOnActionFinished();
		protected abstract void DoInterruptAction();

		//------------- DEBUG UTILITIES -----------------------

		[Button("Start"), FoldoutGroup("Debug/Buttons"), PropertyOrder(1000)]
		private void DebugStartAction()
		{
			_actionPlayer.PlayAction(this);
		}
	}
}