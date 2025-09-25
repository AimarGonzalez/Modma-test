using AG.Gameplay.Combat;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Actions
{
	public class EnemyMoveAction : BaseAction
	{
		//----- Inspector fields -------------------

		[Title("Move settings")]
		[SerializeField]
		private float _distance = 2f;

		[SerializeField, InlineEditor, Required]
		private MMF_Player _feedback;

		[SerializeField, Required]
		private Transform _moveTarget;

		[SerializeField, Min(0.001f)]
		[FoldoutGroup("Advanced")]
		private float _feedbackSpeedMultiplier = 1f;

		//----- Dependencies ----------------
		[Inject] private GameplayWorld _gameplayWorld;


		protected override void Awake()
		{
		}

		//------------- Action methods -------------------

		protected override void DoStartAction(object parameters)
		{
			Subscribe();

			_moveTarget.position = GetNewPosition();

			RootTransform.LookAt(_moveTarget.position);

			PlayFeedbacks();
		}

		private void PlayFeedbacks()
		{
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
			_feedback.SkipToTheEnd();
			DoOnActionFinished();
		}

		private Vector3 GetNewPosition()
		{
			//New position is a random position inside the walkable area
			Vector3 randomPosition;
			MeshRenderer walkableArea = _gameplayWorld.WalkableArea;

			randomPosition.y = RootTransform.position.y;
			randomPosition.z = Random.Range(walkableArea.bounds.min.z, walkableArea.bounds.max.z);
			randomPosition.x = Random.Range(walkableArea.bounds.min.x, walkableArea.bounds.max.x);

			Vector3 direction = (randomPosition - RootTransform.position).normalized;
			randomPosition = RootTransform.position + direction * _distance;

			return randomPosition;
		}
	}
}