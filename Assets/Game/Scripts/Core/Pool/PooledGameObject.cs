using AG.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace AG.Core.Pool
{
	[DisallowMultipleComponent]
	public class PooledGameObject : MonoBehaviour, IDisposable
	{
		[InfoBox("There are multiple PooledGameObject components in the same game object. This is not allowed.", InfoMessageType.Error, nameof(MultipleComponentDetected))]

		[SerializeField]
		[Tooltip("If true, will log an error if the pool is not set")]
		private bool _logMissingPool = true;

		[ShowInInspector, ReadOnly]
		private GameObject _prefab;

		[ShowInInspector, ReadOnly]
		private GameObjectPool _pool;

		public GameObject Prefab => _prefab;

		[ShowInInspector, ReadOnly]
		public bool CreatedOnPool { get; private set; }

		private IPooledComponent[] _subComponents;

		private bool MultipleComponentDetected => gameObject.GetComponents<PooledGameObject>().Length > 1;

		public void Init(GameObject prefab, GameObjectPool pool)
		{
			_prefab = prefab;
			_pool = pool;

			CreatedOnPool = true;
		}

		protected virtual void Awake()
		{
			_subComponents = GetComponentsInChildren<IPooledComponent>();
		}
		
		protected virtual void Start()
		{
			if (!CreatedOnPool)
			{
				TriggerBeforeGetFromPool();
				TriggerAfterGetFromPool();
			}	
		}

		public virtual void TriggerBeforeGetFromPool()
		{
			OnBeforeGetFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnBeforeGetFromPool();
			}
		}

		public virtual void TriggerAfterGetFromPool()
		{
			OnAfterGetFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnAfterGetFromPool();
			}
		}


		public virtual void TriggerReturnToPool()
		{
			OnReturnToPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnReturnToPool();
			}
		}

		public virtual void TriggerDestroyFromPool()
		{
			OnDestroyFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnDestroyFromPool();
			}
		}

		protected virtual void OnBeforeGetFromPool() { }
		protected virtual void OnAfterGetFromPool() { }
		protected virtual void OnReturnToPool() { }
		protected virtual void OnDestroyFromPool() { }

		public void ReleaseToPool()
		{
			if (_pool == null)
			{
				if (_logMissingPool)
				{
					Debug.LogError($"Pool is not set for {gameObject.name}");
				}

				return;
			}

			_pool.Release(this);
		}

		void IDisposable.Dispose()
		{
			ReleaseToPool();
		}
	}
}