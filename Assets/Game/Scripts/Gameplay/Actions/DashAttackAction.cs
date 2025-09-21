using AG.Gameplay.Characters;
using AG.Gameplay.Characters.BodyLocations;
using MoreMountains.Feedbacks;
using SharedLib.ComponentCache;
using SharedLib.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay.Actions
{
	public class DashAttackAction : BaseAction
	{
		//----- Inspector fields -------------------

		[Title("Attack settings")]
		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private float _damage;

		[SerializeField]
		private TagHandle _targetTag;

		[SerializeField, InlineEditor]
		private MMF_Player _feedbacks;

		//----- Components ----------------

		private ColliderListener _colliderListener;
		private Character _character;

		protected override void Awake()
		{
			_colliderListener = Root.Get<ColliderListener>();
			_character = Root.Get<Character>();
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
			_colliderListener.OnTriggerEnterEvent += OnTriggerEnter;
			_feedbacks.Events.OnComplete.AddListener(OnFeedbacksComplete);
		}

		private void Unsubscribe()
		{
			_colliderListener.OnTriggerEnterEvent -= OnTriggerEnter;
			_feedbacks.Events.OnComplete.RemoveListener(OnFeedbacksComplete);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag(_targetTag))
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
			_feedbacks.StopFeedbacks();
			DoOnActionFinished();
		}
	}
}