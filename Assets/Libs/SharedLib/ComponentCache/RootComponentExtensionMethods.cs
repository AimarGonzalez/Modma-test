using UnityEngine;

namespace SharedLib.ComponentCache
{
	public static class RootComponentExtensionMethods
	{
		public static RootComponent GetRoot(this Component component)
		{
			return component.GetComponentInParent<RootComponent>();
		}
	}
}