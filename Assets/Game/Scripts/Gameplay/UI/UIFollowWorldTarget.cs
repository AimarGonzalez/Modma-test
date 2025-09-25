using AG.Core.Pool;
using AG.Gameplay.UI;
using UnityEngine;
using VContainer;

// This script is used to make a UI element follow target in world space

namespace AG.Utils.UI
{
    public class UIFollowWorldTarget : MonoBehaviour, IPooledComponent
    {
        [SerializeField] private Transform _target;
		[SerializeField] private Vector2 _offset;

		[SerializeField] private bool _reparentToCanvas = true;

		// ------------- Dependencies -------------

		[Inject] UIProvider _uiProvider;

        private RectTransform _rectTransform;
        private Camera _worldCamera;
        private Canvas _canvas;
		private Transform _originalParent;

		public void OnBeforeGetFromPool()
		{
			_rectTransform ??= GetComponent<RectTransform>();
			_worldCamera ??= _uiProvider.WorldCamera;
			_canvas ??= _uiProvider.HealthBarsCanvas;

			_originalParent ??= transform.parent;
		}
		
		public void OnAfterGetFromPool()
		{
			if (_reparentToCanvas)
			{
				transform.SetParent(_canvas.transform);
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.identity;
			}
		}


        private void Update()
        {
            if (_target == null || _worldCamera == null)
            {
                return;
            }
            
            Vector3 screenPosition = _worldCamera.WorldToScreenPoint(_target.position);
            _rectTransform.position = screenPosition + new Vector3(_offset.x, _offset.y, _target.position.z);
        }

		public void OnReturnToPool()
		{
			if (_reparentToCanvas)
			{
				transform.SetParent(_originalParent);
			}
        }

        public void OnDestroyFromPool()
        {
        }
    }
}
