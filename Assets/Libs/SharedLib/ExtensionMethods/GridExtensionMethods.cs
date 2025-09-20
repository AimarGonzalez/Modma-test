using UnityEngine;

namespace SharedLib.ExtensionMethods
{
	public static class GridExtensionMethods
	{
		public static Vector3 WorldToTileCenterPosition(this Grid grid, Vector3 worldPosition)
		{
			return grid.GetCellCenterWorld(grid.WorldToCell(worldPosition));
		}
	}
}