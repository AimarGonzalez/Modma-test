using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace SharedLib.ComponentCache
{
	// ComponentIndex creates a cache of all components in a GameObject subtree, storing them in a Dictionary under
	// every base type and every supported interface type. This allows for quick retrieval of components in a tree
	// at the cost of not being able to respond to changes in the subtree.

	public class ComponentIndex
	{
		private readonly Dictionary<Type, object> _componentDictionary = new();

		private void AddObjectReference(Type type, object objectReference)
		{
			// Adds an object to the Dictionary. If there is only one of the type, this is stored directly
			// in the Dictionary. As soon as there's more than one, the Dictionary will contain a List<object>.

			if (_componentDictionary.TryGetValue(type, out object existing))
			{
				// There is already an entry in the dictionary for this type.
				if (existing is List<object>)
				{
					// This is already a list, so add the new object to the list.
					List<object> objectList = (List<object>)existing;
					objectList.Add(objectReference);
				}
				else
				{
					// This is currently a single object, so create a list,
					// add the exiting single object, then the new one.
					List<object> objectList = new List<object> {existing, objectReference};

					_componentDictionary.Remove(type);
					_componentDictionary.Add(type, objectList);
				}
			}
			else
			{
				// Currently no entry exists, so add the object on its own to the dictionary.
				_componentDictionary.Add(type, objectReference);
			}
		}

		public void Create(Transform root)
		{
			_componentDictionary.Clear();

			Component[] allComponents = root.GetComponentsInChildren<Component>(true);

			foreach (Component mb in allComponents)
			{
				if (mb != null)
				{
					Type t = mb.GetType();

					AddObjectReference(t, mb);

					Type[] interfaces = t.GetInterfaces();

					foreach (Type interfaceType in interfaces)
					{
						AddObjectReference(interfaceType, mb);
					}

					IEnumerable<Type> bases = t.GetBaseClasses();

					foreach (Type baseClass in bases)
					{
						AddObjectReference(baseClass, mb);
					}
				}
				else
				{
					Debug.LogError($"Failed to find children components in object. Look for scripts failing to load: {root.gameObject.name}");
				}
			}
		}

		public T Get<T>(bool required = false)
		{
			if (_componentDictionary.TryGetValue(typeof(T), out object entry))
			{
				if (entry is List<object>)
				{
					Debug.LogError($"Get<{typeof(T).Name}> called when multiple components of the type exist!");
				}

				return (T)entry;
			}

			if (required)
			{
				Debug.LogError("Required component type " + typeof(T).Name + " was not found.");
			}

			return default;
		}

		public bool TryGet<T>(out T result, bool required = false)
		{
			result = Get<T>(required);

			return result != null;
		}

		public IEnumerable<T> GetAll<T>(bool required = false)
		{
			if (_componentDictionary.TryGetValue(typeof(T), out object entry))
			{
				if (entry is List<object> listEntry)
				{
					return listEntry.Cast<T>();
				}

				// If the object being retrieved is not a list since it there is only once instance,
				// and the user asked for a list, create a list here with just the single entry inside.
				return new List<T> {(T)entry};
			}

			if (required)
			{
				Debug.LogError("Required component type " + typeof(T).Name + " was not found.");
			}

			return Enumerable.Empty<T>();
		}
	}
}