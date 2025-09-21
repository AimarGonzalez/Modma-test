using System;
using UnityEngine;

namespace SharedLib.Physics
{
	public class ColliderListener : MonoBehaviour
	{
		public event Action<Collider> OnTriggerEnterEvent;
		public event Action<Collider> OnTriggerExitEvent;
		public event Action<Collider> OnTriggerStayEvent;

		public event Action<Collider> OnCollisionEnterEvent;
		public event Action<Collider> OnCollisionExitEvent;
		public event Action<Collider> OnCollisionStayEvent;

		public event Action OnMouseDownEvent;


		//--------------------------------

		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnTriggerExitEvent?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			OnTriggerStayEvent?.Invoke(other);
		}

		//--------------------------------

		private void OnCollisionEnter(Collision other)
		{
			OnCollisionEnterEvent?.Invoke(other.collider);
		}

		private void OnCollisionStay(Collision other)
		{
			OnCollisionStayEvent?.Invoke(other.collider);
		}

		private void OnCollisionExit(Collision other)
		{
			OnCollisionExitEvent?.Invoke(other.collider);
		}

		//--------------------------------

		private void OnMouseDown()
		{
			OnMouseDownEvent?.Invoke();
		}
	}
}