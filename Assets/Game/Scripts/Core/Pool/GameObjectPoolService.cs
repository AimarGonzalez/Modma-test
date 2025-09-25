using AG.Core.UI;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace AG.Core.Pool
{
	public class GameObjectPoolService : MonoBehaviour
	{
		[Inject]
		private IObjectResolver _vcontainer;
		
		private Dictionary<GameObject, GameObjectPool> _pools = new();

		[SerializeField]
		private bool _showDebugPanel = false;

		public T Get<T>(T component, Transform parent) where T : MonoBehaviour
		{
			return Get(component, parent, Vector3.zero, Quaternion.identity);
		}

		public T Get<T>(T component, Transform parent, Vector3 position, Quaternion rotation) where T : MonoBehaviour
		{
			return Get(component, parent, position, rotation, inWorldSpace: false);
		}

		public T Get<T>(T component, Transform parent, Vector3 position, Quaternion rotation, bool inWorldSpace) where T : MonoBehaviour
		{
			GameObject pooledGameObject = component.gameObject;
			PooledGameObject instance = Get(pooledGameObject, parent, active: true, position, rotation, inWorldSpace);
			return instance.GetComponentInChildren<T>(includeInactive: true);
		}

		public T Get<T>(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool inWorldSpace) where T : MonoBehaviour
		{
			PooledGameObject instance = Get(prefab, parent, active: true, position, rotation, inWorldSpace);
			return instance.GetComponentInChildren<T>(includeInactive: true);
		}
		
		public T Get<T>(GameObject prefab, Transform parent, bool active, Vector3 position, Quaternion rotation, bool inWorldSpace) where T : MonoBehaviour
		{
			PooledGameObject instance = Get(prefab, parent, active, position, rotation, inWorldSpace);
			return instance.GetComponentInChildren<T>(includeInactive: true);
		}

		public PooledGameObject Get(GameObject prefab)
		{
			return Get(prefab, parent: null, active: true, Vector3.zero, Quaternion.identity, inWorldSpace: false);
		}

		public PooledGameObject Get(GameObject prefab, Transform parent, bool active, Vector3 position, Quaternion rotation, bool inWorldSpace)
		{
			if (prefab == null)
			{
				Debug.LogError($"Prefab reference is null");
				return null;
			}

			GameObjectPool pool = GetOrCreatePool(prefab);
			return pool.Get(prefab, parent, active, position, rotation, inWorldSpace);
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
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			if (instance.CreatedOnPool)
			{
				instance.ReleaseToPool();
			}
			else
			{
				GameObjectPool pool = GetOrCreatePool(instance.Prefab);
				pool.Release(instance);
			}

			
		}

		private GameObjectPool GetOrCreatePool(GameObject prefab)
		{
			if (!_pools.TryGetValue(prefab, out GameObjectPool pool))
			{
				pool = new GameObjectPool(_vcontainer, transform);
				_pools[prefab] = pool;
			}
			return pool;
			
		}

		private void OnGUI()
		{
			if (!_showDebugPanel)
			{
				return;
			}
			
			GUIUtils.PushFontSize(35);
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);
			
			// area centered on the middle right of the screen
			GUILayout.BeginArea(new Rect(Screen.width - 400, Screen.height*0.5f - 200f, 400, 500), GUI.skin.box);

			GUILayout.BeginVertical();
			foreach (var pool in _pools)
			{
				GUILayoutUtils.Label($"{pool.Key.name}: {pool.Value.NumActiveObjects} / {pool.Value.NumTotalObjects}");
			}
			GUILayout.EndVertical();

			GUILayout.EndArea();
			
			GUIUtils.PopFontSize();
		}
	}
}