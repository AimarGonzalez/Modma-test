using AG.Core.UI;
using DG.Tweening;
using SharedLib.ExtensionMethods;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AG.Gameplay.UI
{
	[ExecuteInEditMode]
	public class ProgressBar : MonoBehaviour
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image _animatedBarGraphic;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[Range(0f, 1f)]
		[SerializeField] private float _inspectorValue = 1f;

		[SerializeField] private float _lerpTime = 0.2f;
		
		[Tooltip("Minimum value change to trigger a view update")]
		[SerializeField] private float _minimumChange = 0.01f;

		private float _value;
		private bool _valueChanged;

		public float Value
		{
			get => _value;
			set
			{
				if (!_value.IsAlmostEqual(value, _minimumChange))
				{
					_value = value;
					_valueChanged = true;
				}
			}
		}

		private void Start()
		{
			_value = _inspectorValue;
			SetFillRatio(_inspectorValue);
		}

		void OnValidate()
		{
			SetFillRatio(_inspectorValue);
		}

		protected virtual void Update()
		{
			if (_valueChanged)
			{
				SetFillRatio(_value);
				_valueChanged = false;
			}
		}

		private void SetFillRatio(float value)
		{
			if (Application.isPlaying)
			{
				_animatedBarGraphic.DOFillAmount(value, _lerpTime);
			}
			else
			{
				_animatedBarGraphic.fillAmount = value;
			}
		}
	}
}
