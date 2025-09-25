using AG.Core.Pool;
using AG.Gameplay.Characters;
using AG.Gameplay.Settings;
using InspectorGadgets.Attributes;
using SharedLib.ComponentCache;
using SharedLib.ExtensionMethods;
using SharedLib.StateMachines;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace AG.Gameplay.UI
{
	public class HealthBarController : SubComponent, IPooledComponent
	{
		[Header("UI References")]
		[SerializeField] private TextMeshProUGUI _text;
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField, Required] private Image _healthBarFill;

		[Tooltip("The UI Image component that represents the health bar frame")]
		[SerializeField, Required] private Image _healthBarFrame;
		[SerializeField, Required] private List<StateId> _visibleStateIds;

		// ------------- Components -------------
		private ProgressBar _progressBar;

		// ------------- Dependencies -------------

		[Inject]
		private GameSettings _gameSettings;

		// ------------- Private fields -------------

		private UISettings _uiSettings;
		private Character _character;
		private float _lastHealth;


		public void OnBeforeGetFromPool()
		{
			_progressBar = Root.Get<ProgressBar>();
			_character = Root.Get<Character>();
			
			_uiSettings = _gameSettings.UISettings;
		}

		public void OnAfterGetFromPool()
		{
			UpdateView();

			_character.OnHealthChanged += OnHealthChanged;
			_character.OnStateChanged += OnStateChanged;
		}

		public void OnReturnToPool()
		{
			_character.OnHealthChanged -= OnHealthChanged;
			_character.OnStateChanged -= OnStateChanged;
		}

		public void OnDestroyFromPool()
		{
		}

		private void UpdateView()
		{
			UpdateColor();
			UpdateFillRatio();
			UpdateVisibility();
		}

		private void OnHealthChanged(float prevHealth, float newHealth)
		{
			if (!_character.Health.IsAlmostEqual(_lastHealth))
			{
				return;
			}

			UpdateFillRatio();

			// play vfx
		}
		
		private void OnStateChanged(StateId prevState, StateId newState)
		{
			UpdateVisibility();
		}

		private void UpdateColor()
		{
			if (_character == null)
			{
				return;
			}

			SetColor(_character.Team);
		}

		private void SetColor(Team team)
		{
			UISettings.HealthBarColors healthBarColors;
			if (team == Team.Enemy)
			{
				healthBarColors = _uiSettings.EnemyHealthBarColors;
			}
			else
			{
				healthBarColors = _uiSettings.AllyHealthBarColors;
			}

			_healthBarFrame.color = healthBarColors.FrameColor;
			_healthBarFill.color = healthBarColors.BarColor;
		}


		private void UpdateFillRatio()
		{
			if (_text)
			{
				_text.text = _character.Health.ToString("F0");
			}
			_progressBar.Value = _character.Health / _character.MaxHealth;
		}

		private void UpdateVisibility()
		{
			gameObject.SetActive(_visibleStateIds.Contains(_character.StateId));
		}
	}
}