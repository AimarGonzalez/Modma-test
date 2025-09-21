using System;
using System.Collections.Generic;

namespace SharedLib.ExtensionMethods
{
	public static class IEnumerableExtensionMethods
	{
		public static IEnumerable <T> AsEnumerable<T>(this T item)
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