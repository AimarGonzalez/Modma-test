using System;
using AG.Gameplay.Balls;
using AG.Gameplay.GamePhysics;
using AG.Gameplay.Services;
using AG.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace AG.Gameplay.Actions
{
    public class SwingBatAction : BaseAction
    {
        //----- Inspector fields -------------------

        [SerializeField]
        private string _transitionName = "SwingBat";

        [SerializeField, Unit(Units.DegreesPerSecond)]
        private float _angularSpeed = 1000f;

        [SerializeField, Range(-180, 180)]
        private float _minimumRotationArc = 90f;

        [SerializeField]
        private Ease _rotationEase = Ease.Linear;

        [SerializeField, Required]
        private Transform _swingVFXAnchor;

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
            Animator.SetTrigger(_transitionName);

            //TODO: try hit ball
            SwingResultData result = _physicsService.SwingBat(Root);

            //TODO: Play vfx
            PlaySwingVfx(result);

            RotateToBall(result);
        }

        private void PlaySwingVfx(SwingResultData result)
        {
            _swingVFXAnchor.position = result.ContactPoint;
            _swingVFXAnchor.rotation = result.Rotation;

            switch (result.Result)
            {
                case SwingResult.Miss:
                    Root.SwingMissFeedback.PlayFeedbacks();
                    break;
                case SwingResult.Success_TooLate:
                    Root.SwingLateFeedback.PlayFeedbacks();
                    break;
                case SwingResult.Success_Perfect:
                    Root.SwingPerfectFeedback.PlayFeedbacks();
                    break;
                case SwingResult.Success_TooEarly:
                    Root.SwingEarlyFeedback.PlayFeedbacks();
                    break;
            }
        }

        private void RotateToBall(SwingResultData result)
        {
            FaceBallRotatingClockwise(result.Ball);
        }

        private void FaceBallRotatingClockwise(Ball ball)
        {
            Vector3 directionToTarget = (ball.Position - Root.Transform.position).normalized;
            float rotationArc = Vector3.SignedAngle(Root.Transform.forward, directionToTarget, Vector3.up);

            // Adjust rotation to perform almost a full spin.
            if (rotationArc < _minimumRotationArc)
            {
                rotationArc += 360f;
            }

            // Tween at constant speed
            Quaternion startingRotation = Root.Transform.rotation;
            DOVirtual.Float(0, rotationArc, _angularSpeed, (value) =>
                {
                    Root.Transform.rotation = startingRotation * Quaternion.Euler(0, value, 0); ;
                })
                .SetEase(_rotationEase)
                .SetSpeedBased(true)
                .SetId(this);
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