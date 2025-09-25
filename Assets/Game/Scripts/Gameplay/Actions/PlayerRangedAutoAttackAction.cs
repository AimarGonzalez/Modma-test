using AG.Gameplay.Characters;
using AG.Gameplay.Characters.Components;
using Animancer;
using AG.Gameplay.Cards.CardStats;
using AG.Gameplay.Characters.MonoBehaviours.BodyLocations;
using Modma.Game.Scripts.Gameplay.Projectiles;
using NUnit.Framework;
using SharedLib.ExtensionMethods;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Actions
{
	public class PlayerRangedAutoAttackAction : BaseAction
	{

		// ------------- Inspector fields -------------
		[SerializeField] private StringAsset _shootEvent;
		[SerializeField] private StringAsset _shootEndEvent;
		[SerializeField] private AttackStatsSO _projectileStats;

		// ------------- Components -------------
		private AnimancerComponent _animancer;
		private PlayerAnimations _playerAnimations;
		private Character _character;
		private Transform _projectileSourceLocation;

		// ------------- Dependencies -------------
		[Inject] private ProjectileFactory _projectileFactory;

		// ------------- Private fields -------------

		private Character _attackTarget;

		protected override void Awake()
		{
			_playerAnimations = Root.Get<PlayerAnimations>();
			_animancer = Root.Get<AnimancerComponent>();
			_character = Root.Get<Character>();
			_projectileSourceLocation = Root.Get<ProjectileSourceLocation>().transform;
		}

		protected override void DoStartAction(object parameters)
		{
			_attackTarget = parameters as Character;
			Debug.Assert(_attackTarget != null, "received null action parameters");

			_playerAnimations.PlayRangedAttack();

			Subscribe();
		}

		private void Subscribe()
		{
			_animancer.Events.AddTo(_shootEvent, OnShoot);
			_animancer.Events.AddTo(_shootEndEvent, OnShootEnd);
		}

		private void Unsubscribe()
		{
			_animancer.Events.Remove(_shootEvent, OnShoot);
			_animancer.Events.Remove(_shootEndEvent, OnShootEnd);
		}

		private void OnShoot()
		{
			
			ProjectileController projectile = _projectileFactory.BuildProjectile(_projectileSourceLocation, _projectileStats.ProjectilePrefab);
			Vector3 direction = _projectileSourceLocation.FlatDirection(_attackTarget.RootTransform);
			projectile.Initialize(_character, direction, _projectileStats);

			//TODO
			// - do damage
			// - play vfx
			// - play sound
		}

		private void OnShootEnd()
		{
			_playerAnimations.PlayAimingIdle();
			Status = ActionStatus.Finished;
		}

		protected override void DoOnActionFinished()
		{
			Unsubscribe();
		}

		protected override void DoInterruptAction()
		{
			Unsubscribe();
		}
	}
}