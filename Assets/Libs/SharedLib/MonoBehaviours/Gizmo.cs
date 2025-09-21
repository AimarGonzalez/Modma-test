

using UnityEngine;

namespace SharedLib.MonoBehaviours
{
	public class Gizmo : MonoBehaviour
	{
		public enum GizmoType
		{
			Sphere,
			SquareWire,
			Cube,
			CubeWire
		}


		// ------------ Inspector fields -----------------
		// Add fields to select the type of gizmo and color

		[SerializeField]
		private GizmoType _gizmoType;

		[SerializeField]
		private Color _color;

		[SerializeField]
		private float _size = 0.5f;

		private Color _previousColor;

		private void OnDrawGizmos()
		{
			Gizmos.color = _color;
			switch (_gizmoType)
			{
				case GizmoType.Sphere:
					Gizmos.DrawSphere(transform.position, _size);
					break;
				case GizmoType.SquareWire:
					Gizmos.DrawWireCube(transform.position, new Vector3(_size, _size, _size));
					break;
				case GizmoType.Cube:
					Gizmos.DrawCube(transform.position, new Vector3(_size, _size, _size));
					break;
				case GizmoType.CubeWire:
					Gizmos.DrawWireCube(transform.position, new Vector3(_size, _size, _size));
					break;
			}
			_previousColor = _color;
		}
	}
}