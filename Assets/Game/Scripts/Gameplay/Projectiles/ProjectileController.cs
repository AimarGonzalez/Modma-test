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
namespace AG.Gameplay.Projectiles
{
	public class ProjectileController : SubComponent
	{

		//----- Dependencies ----------------
		[Inject] private GameplayWorld _gameplayWorld;
		[Inject] private GameSettings _gameSettings;


		//----- Components ----------------
		private PooledGameObject _pooledGameObject;
		private ColliderListener _collisionListener;
		private Rigidbody _rigidbody;
		private TrailRenderer _trailRenderer;


		//----- Private fields ----------------

		private CombatSettingsSO _combatSettings;
		private Vector3 _direction;
		private float _speed;
		private Character _source;
		private AttackStatsSO _projectileStats;
		private int _projectileTargetLayer;
		private LayerMask _wallsLayer;
		private LayerMask _characterLayers;

		public void OnEnable()
		{
			FetchDependencies();
		}

		private void FetchDependencies()
		{
			_pooledGameObject = Root.Get<PooledGameObject>();
			_collisionListener = Root.Get<ColliderListener>();
			_rigidbody = Root.Get<Rigidbody>();
			_trailRenderer = Root.Get<TrailRenderer>();
		}

		public void Initialize(Character source, Vector3 direction, AttackStatsSO projectileStats)
		{
			_source = source;
			_direction = direction;
			_direction.y = 0;
			_direction.Normalize();
			
			_projectileStats = projectileStats;
			
			_speed = projectileStats.ProjectileSpeed;
			
			//Get collision layers
			_projectileTargetLayer = _gameSettings.CombatSettingsSO.GetProjectileDamageLayer(_source);
			_wallsLayer = _gameSettings.CombatSettingsSO.WallsLayer;
			_characterLayers = _gameSettings.CombatSettingsSO.CharacterLayers;

			//Set direction to both components to avoid 1st frame of desynchronization.
			RootTransform.rotation = Quaternion.LookRotation(_direction);
			_rigidbody.rotation = Quaternion.LookRotation(_direction);

			//Listen to collisions
			_collisionListener.OnTriggerEnterEvent += OnTriggerEnter;	
			_collisionListener.OnCollisionEnterEvent += OnTriggerEnter;
			_collisionListener.gameObject.layer = _projectileTargetLayer; // convert from layer mask to layer
		}

		public void OnDisable()
		{
			_collisionListener.OnCollisionEnterEvent -= OnTriggerEnter;
			_collisionListener.OnTriggerEnterEvent -= OnTriggerEnter;
			
			_trailRenderer.Clear();
		}

		private void FixedUpdate()
		{
			Vector3 newPosition = _rigidbody.position + _direction * (_speed * Time.deltaTime);
			_rigidbody.MovePosition(newPosition);
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
				//Debug.Log(" >> Projectile hit character");
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
			return _characterLayers.MMContains(other.gameObject);
		}
	}
}