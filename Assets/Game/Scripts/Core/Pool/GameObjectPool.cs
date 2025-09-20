using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AG.Core.Pool
{
	public class GameObjectPool
	{
		private Transform _parent;
		private IObjectResolver _vcontainer;

		[ShowInInspector, ReadOnly]
		private int _numActiveObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private int _numPooledObjects = 0;
		
		private int _numInstancedObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private Queue<GameObject> _queue = new();

		public int NumActiveObjects => _numActiveObjects;
		public int NumTotalObjects => _numActiveObjects + _numPooledObjects;
		 

		public GameObjectPool(IObjectResolver vcontainer, Transform parent)
		{
			_vcontainer = vcontainer;
			_parent = parent;
		}

		public PooledGameObject Get(GameObject prefab, Transform parent)
		{
			return Get(prefab, parent, active: true, Vector3.zero, Quaternion.identity, inWorldSpace: false);
		}

		public PooledGameObject Get(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
		{ 
			return Get(prefab, parent, active: true, position, rotation, inWorldSpace: false);
		} 

		public PooledGameObject Get(GameObject prefab, Transform parent, bool active, Vector3 position, Quaternion rotation, bool inWorldSpace)
		{
			GameObject instance;
			if (_queue.Count > 0)
			{
				instance = _queue.Dequeue();
				_numPooledObjects--;
			}
			else
			{
				_numInstancedObjects++;
				instance = _vcontainer.Instantiate(prefab);
				instance.name = $"{prefab.name}-{_numInstancedObjects}";
			}
			
			PooledGameObject pooledGameObject = instance.GetComponent<PooledGameObject>();

			_numActiveObjects++;

			pooledGameObject.Init(prefab, this);
			
			Transform transform = instance.transform;
			transform.SetParent(parent, false);

			if (inWorldSpace)
			{
				transform.position = position;
				transform.rotation = rotation;
			}
			else
			{
				transform.localPosition = position;
				transform.localRotation = rotation;
			}

			pooledGameObject.TriggerBeforeGetFromPool(); // UnitRoot.CreateUnit
			instance.SetActive(active);
			pooledGameObject.TriggerAfterGetFromPool();

			return pooledGameObject;
		}

		public void Release(GameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			PooledGameObject prefabInfo = instance.GetComponent<PooledGameObject>();
			Release(prefabInfo);
		}

		public void Release(PooledGameObject instance)
		{
			GameObject gameObject = instance.gameObject;
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			if (instance.CreatedOnPool)
			{
				_numActiveObjects--;
			}


			instance.TriggerReturnToPool();

			gameObject.SetActive(false);
			gameObject.transform.SetParent(_parent);

			if (_queue.Contains(gameObject))
			{
				Debug.LogError($"Prefab reference is already in the pool: {instance.name}");
				return;
			}

			_queue.Enqueue(gameObject);
			_numPooledObjects++;
		}
	}
}