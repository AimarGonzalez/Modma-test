﻿
using UnityEngine;

namespace SharedLib.ExtensionMethods
{
	public static class Vector3ExtensionMethods
	{
		public static float DistanceSquared(this Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(b - a);
		}
	}
}