using UnityEngine;

namespace SharedLib.ExtensionMethods
{
	public static class FloatExtensionMethods
	{
		public static float Divide_NoNaN(this float value, float divisor)
		{
			if (divisor == 0)
			{
				return 0;
			}

			return value / divisor;
		}

		public static bool IsAlmostEqual(this float a, float b, float epsilon = 0.0001f)
		{
			return Mathf.Abs(a - b) < epsilon;
		}
	}
}