using System.Collections.Generic;
using System;
using UnityEngine;

namespace SharedLib.ComponentCache
{
	public class RootComponent : MonoBehaviour
	{
		public event Action OnDestroyed;
		
		private ComponentIndex _components = null;

		public ComponentIndex Components
		{
			get
			{
				if (_components == null)
				{
					CreateComponentCache();
				}

				return _components;
			}
		}

		public void CreateComponentCache()
		{
			_components = new ComponentIndex();
			_components.Create(transform);
		}

		public TComponent Get<TComponent>(bool required = true) => Components.Get<TComponent>(required);
		
		public bool TryGet<TComponent>(out TComponent result, bool required = true) => Components.TryGet<TComponent>(out result, required);

		public IEnumerable<TComponent> GetAll<TComponent>(bool required = true) => Components.GetAll<TComponent>(required);

		private void OnDestroy()
		{
			OnDestroyed?.Invoke();
		}
	}
}