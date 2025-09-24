
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SharedLib.ExtensionMethods
{
	public static class Vector3ExtensionMethods
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(this Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(b - a);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FlatDirection(this Transform origin, Transform target)
		{
			return origin.position.FlatDirection(target.position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FlatDirection(this Transform origin, Vector3 target)
		{
			return origin.position.FlatDirection(target);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FlatDirection(this Vector3 origin, Transform target)
		{
			return origin.FlatDirection(target.position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FlatDirection(this Vector3 origin, Vector3 target)
		{
			Vector3 direction = target - origin;
			direction.y = 0;
			direction.Normalize();
			return direction;
		}
	}
}