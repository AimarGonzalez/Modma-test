using UnityEngine;

namespace SharedLib.ComponentCache
{
	public abstract class SubComponent : MonoBehaviour
	{
		private RootComponent _root;
		private Transform _rootTransform;

		public RootComponent Root
		{
			get
			{
				if (_root == null)
				{
					_root = GetComponentInParent<RootComponent>(true);
					Debug.Assert(_root != null, "SubComponent can't find a parent RootComponent!");
				}

				return _root;
			}
		}

		public Transform RootTransform
		{
			get
			{
				_rootTransform ??= Root.transform;
				return _rootTransform;
			}
		}
	}
}