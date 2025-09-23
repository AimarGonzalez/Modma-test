using AG.Core.UI;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AG.Gameplay.Characters.BodyLocations
{
	// Provides Radius based on geometry, and updates the radius of its collider accordingly.
	// Provides a VerticalCenter to be used for projectiles.
	public class Body : SubComponent, IDebugPanelDrawer
	{
		// ------------- Inspector fields -------------
		[SerializeField]
		private float _radiusOffset;

		[SerializeField]
		private MeshRenderer[] _subSetOfMeshRenderers;

		// ------------- Private fields -------------
		private CapsuleCollider _capsuleCollider;


		[ShowInInspector]
		private Vector3 _verticalCenter;

		[ShowInInspector]
		private float _radius;

		private Bounds _bounds;

		public Vector3 VerticalCenter => _verticalCenter;
		public float Radius => _radius;

		public Vector3 ProjectileTargetPosition => VerticalCenter;
		public Vector3 MovementTargetPosition => transform.position;

		protected void Awake()
		{
			_capsuleCollider = GetComponent<CapsuleCollider>();

			_bounds = CalculateAggregateBounds();
			_radius = Mathf.Max(_bounds.size.x, _bounds.size.z) / 2 + _radiusOffset;
			_verticalCenter = _bounds.center;
		}

		private Bounds CalculateAggregateBounds()
		{
			MeshRenderer[] bodyMeshRenderers = _subSetOfMeshRenderers.Length > 0 ? _subSetOfMeshRenderers : Root.GetAll<MeshRenderer>().ToArray();

			Bounds aggregateBounds = new Bounds();
			foreach (MeshRenderer meshRenderer in bodyMeshRenderers)
			{
				aggregateBounds.Encapsulate(meshRenderer.localBounds);
			}

			return aggregateBounds;
		}


		// Calculates the position to target the body from a given attacker position
		// Useful for big square units
		public Vector3 GetTargetPositionOnSquareUnit(Vector3 attackerPosition)
		{
			Vector3 bodyPosition = VerticalCenter;

			// Calculate direction from attacker to body center
			Vector3 direction = (bodyPosition - attackerPosition).normalized;

			// Create a ray from attacker position towards body center
			Ray ray = new Ray(attackerPosition, direction);

			// Check if ray intersects with bounding box
			if (_bounds.IntersectRay(ray, out float distance))
			{
				// Return the intersection point
				return attackerPosition + direction * distance;
			}

			// Fallback: if no intersection (shouldn't happen in normal cases), return center
			return bodyPosition;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(VerticalCenter, 0.1f);
		}

		private void OnValidate()
		{
			if (_capsuleCollider)
			{
				_capsuleCollider.radius = _radius;
			}
		}

		public void AddDebugProperties(List<GUIUtils.Property> properties)
		{
			properties.Add(new("Body.Radius", Radius.ToString()));
		}
	}
}