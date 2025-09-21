using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class MoveAction : BaseAction
	{
		//----- Inspector fields -------------------

		[Title("Move settings")]
		[SerializeField, InlineEditor]
		private MMF_Player _feedback;

		[SerializeField]
		private float _feedbackSpeedMultiplier = 1f;
		//----- Components ----------------


		protected override void Awake()
		{
		}

		//------------- Action methods -------------------

		protected override void DoStartAction(object parameters)
		{
			Subscribe();

			_feedback.DurationMultiplier = 1 / Mathf.Max(_feedbackSpeedMultiplier, 0.001f);
			_feedback.Initialization();

			_feedback.PlayFeedbacks();
		}

		private void Subscribe()
		{
			_feedback.Events.OnComplete.AddListener(OnFeedbacksComplete);
		}

		private void Unsubscribe()
		{
			_feedback.Events.OnComplete.RemoveListener(OnFeedbacksComplete);
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
			_feedback.StopFeedbacks();
			DoOnActionFinished();
		}
	}
}