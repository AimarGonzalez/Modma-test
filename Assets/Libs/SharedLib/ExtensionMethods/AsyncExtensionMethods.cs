using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SharedLib.ExtensionMethods
{
	public static class AsyncExtensionMethods
	{
		public static void RunAsync(this Task task)
		{
			// Safety net to avoid unhandled exceptions
			task.ContinueWith(t =>
			{
				if (t.IsFaulted)
				{
					Debug.LogError(t.Exception);
				}
			});
		}


		public static void RunAsync(this Awaitable awaitable)
		{
			_ = RunSafeAsync(awaitable);
		}

		private static async Awaitable RunSafeAsync(Awaitable awaitable)
		{
			try
			{
				await awaitable;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
	}
}