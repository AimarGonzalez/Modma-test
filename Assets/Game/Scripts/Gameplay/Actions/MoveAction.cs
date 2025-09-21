using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class MoveAction : BaseAction
	{
		//----- Inspector fields -------------------

		[Title("Move settings")]
		[SerializeField]
		private float _speed = 1f;

		[SerializeField, InlineEditor]
		private MMF_Player _feedbacks;

		//----- Components ----------------


		protected override void Awake()
		{
		}

		//------------- Action methods -------------------

		protected override void DoStartAction(object parameters)
		{
			Subscribe();

			_feedbacks.DurationMultiplier = 1 / Mathf.Max(_speed, 0.001f);
			_feedbacks.Initialization();

			_feedbacks.PlayFeedbacks();
		}

		private void Subscribe()
		{
			_feedbacks.Events.OnComplete.AddListener(OnFeedbacksComplete);
		}

		private void Unsubscribe()
		{
			_feedbacks.Events.OnComplete.RemoveListener(OnFeedbacksComplete);
		}

		private void OnFeedbacksComplete()
		{
			Status = ActionStatus.Finished;
		}

		protected override ActionStatus DoUpdateAction()
		{
			return Status;
		}

		protected override void DoOnActionFinished()
		{
			Unsubscribe();
		}

		protected override void DoInterruptAction()
		{
			_feedbacks.StopFeedbacks();
			DoOnActionFinished();
		}
	}
}