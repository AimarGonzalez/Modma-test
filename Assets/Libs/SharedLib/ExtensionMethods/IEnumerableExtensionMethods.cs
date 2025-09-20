using System;
using System.Collections.Generic;

namespace SharedLib.ExtensionMethods
{
	public static class IEnumerableExtensionMethods
	{
		public static IEnumerable <T> SingleItemAsEnumerable<T>(this T item)
		{
			yield return item; 
		}
		
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}