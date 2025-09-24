using AG.Core.Pool;
using AG.Gameplay.Cards.CardStats;
using AG.Gameplay.Characters;
using AG.Gameplay.Combat;
using AG.Gameplay.Settings;
using MoreMountains.Tools;
using SharedLib.ComponentCache;
using SharedLib.Physics;
using UnityEngine;
using VContainer;
namespace Modma.Game.Scripts.Gameplay.Projectiles
{
	public class ProjectileController : SubComponent
	{

		//----- Dependencies ----------------
		[Inject] private ArenaWorld _arenaWorld;
		[Inject] private GameSettings _gameSettings;


		//----- Components ----------------
		private PooledGameObject _pooledGameObject;
		private ColliderListener _collisionListener;


		//----- Private fields ----------------

		private CombatSettingsSO _combatSettings;
		private Vector3 _direction;
		private float _speed;
		private Character _source;
		private AttackStatsSO _projectileStats;
		private LayerMask _projectileTargetLayer;
		private LayerMask _wallsLayer;

		public void Initialize(Character source, Vector3 direction, AttackStatsSO projectileStats)
		{
			_source = source;
			_direction = direction;
			_direction.y = 0;
			_direction.Normalize();
			_projectileStats = projectileStats;

			_projectileTargetLayer = _gameSettings.CombatSettingsSO.GetProjectileTargetLayer(_source.Team);
			_wallsLayer = _gameSettings.CombatSettingsSO.WallsLayer;

			_pooledGameObject = Root.Get<PooledGameObject>();
		}

		public void OnEnable()
		{
			//Set direction
			RootTransform.rotation = Quaternion.LookRotation(_direction);

			//Listen to collisions
			_collisionListener.OnTriggerEnterEvent += OnTriggerEnter;	
		}

		public void OnDisable()
		{
			_collisionListener.OnTriggerEnterEvent -= OnTriggerEnter;
		}

		private void Update()
		{
			RootTransform.position += _direction * _speed * Time.deltaTime;
		}

		private void OnTriggerEnter(Collider other)
		{

			if (IsWall(other))
			{
				// Play hit vfx
				Debug.Log(" >> Projectile hit wall");

				_pooledGameObject.ReleaseToPool();
				return;
			}

			if (IsTargetCharacter(other))
			{
				// Play hit vfx
				Debug.Log(" >> Projectile hit character");
				Character targetCharacter = other.GetRoot().Get<Character>();
				targetCharacter.Hit(_source, _projectileStats.Damage);

				_pooledGameObject.ReleaseToPool();
				return;
			}
		}

		private bool IsWall(Collider other)
		{
			return _wallsLayer.MMContains(other.gameObject);
		}

		private bool IsTargetCharacter(Collider other)
		{
			return _projectileTargetLayer.MMContains(other.gameObject);
		}
	}
}