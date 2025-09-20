using AG.Gameplay.Balls;
using AG.Gameplay.Services;
using AG.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace AG.Gameplay.Actions
{
	public class SmashedAction : BaseAction
	{
		//----- Inspector fields -------------------

		[SerializeField]
		private string _transitionName = "FallToGround";

		//----- External dependencies ----------------

		private GamePhysicsService _physicsService;

		//----- Internal variables -------------------

		private void Start()
		{
			_physicsService = ServiceLocator.Instance.Get<GamePhysicsService>();
		}

		//--------------------------------

		protected override void DoStartAction(object parameters)
		{
			RotateToBall();
            
			Animator.SetTrigger(_transitionName);
		}

		private void RotateToBall()
		{
			(Vector3 contactPoint, Quaternion rotation) = _physicsService.GetContactPointAndRotation(Root);
			Root.Transform.rotation = rotation;
		}

		protected override ActionStatus DoUpdateAction()
		{
			return Status;
		}

		protected override void DoFixedUpdateAction()
		{
		}

		protected override void DoOnActionFinished()
		{
			DOTween.Kill(this);
		}

		public override void ResetComponent()
		{
			base.ResetComponent();

			DOTween.Kill(this);
		}
	}
}