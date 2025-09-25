using AG.Gameplay.Characters;
using AG.Gameplay.Combat;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using SharedLib.ComponentCache;
using SharedLib.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Actions
{
	public class DashAttackAction : BaseAction
	{
		//----- Inspector fields -------------------

		[Title("Attack settings")]
		[SerializeField]
		private float _distance = 2.5f;

		[SerializeField]
		private float _damage;

		[SerializeField]
		private LayerMask _damagedLayers;

		[SerializeField]
		private ColliderListener _colliderListener;

		[SerializeField, InlineEditor]
		private MMF_Player _feedback;

		[SerializeField, Required]
		private Transform _attackTarget;

		[SerializeField, Min(0.001f)]
		[FoldoutGroup("Advanced")]
		private float _feedbackSpeedMultiplier = 1f;


		//----- Components ----------------

		private Character _character;

		//----- Dependencies ----------------
		[Inject] private GameplayWorld _gameplayWorld;

		protected override void Awake()
		{
			_character = Root.Get<Character>();
		}


		//------------- Action methods -------------------

		protected override void DoStartAction(object parameters)
		{
			Subscribe();

			_attackTarget.position = GetNewPosition();

			RootTransform.LookAt(_attackTarget.position);

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
			_colliderListener.OnTriggerEnterEvent += OnTriggerEnter;
			_feedback.Events.OnComplete.AddListener(OnFeedbacksComplete);
		}

		private void Unsubscribe()
		{
			_colliderListener.OnTriggerEnterEvent -= OnTriggerEnter;
			_feedback.Events.OnComplete.RemoveListener(OnFeedbacksComplete);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_damagedLayers.MMContains(other.gameObject))
			{
				return;
			}

			Debug.Log("OnTriggerEnter");
			Character targetCharacter = other.GetRoot().Get<Character>();
			targetCharacter.Hit(_character, _damage);
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
			//New position is in the direction of the Player
			Vector3 direction = (_gameplayWorld.Player.RootTransform.position - RootTransform.position).normalized;
			if (direction == Vector3.zero)
			{
				direction = Vector3.forward;
			}
			Vector3 targetPosition = RootTransform.position + direction * _distance;

			return targetPosition;
		}
	}
}