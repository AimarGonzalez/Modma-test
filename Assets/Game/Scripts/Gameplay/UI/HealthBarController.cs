using AG.Core.UI;
using AG.Gameplay.Characters;
using AG.Gameplay.Settings;
using AG.Gameplay.Units;
using SharedLib.ComponentCache;
using SharedLib.ExtensionMethods;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace AG.Gameplay.UI
{
	public class HealthBarController : SubComponent
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image _healthBarFill;

		[Tooltip("The UI Image component that represents the health bar frame")]
		[SerializeField] private Image _healthBarFrame;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[SerializeField] private Team _team = Team.Player;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[Range(0f, 1f)]
		[SerializeField] private float _healthRatio = 1f;

		// ------------- Dependencies -------------

		[Inject]
		private GameSettings _gameSettings;
		private UISettings UISettings => _gameSettings.UISettings;
		private Character _character;

		// ------------- Private fields -------------

		private float _lastHealth;

		private void Awake()
		{
			_character = Root.Get<Character>();
		}

		private void OnEnable()
		{
			_character.OnHealthChanged += OnHealthChanged;
		}
		
		private void OnDisable()
		{
			_character.OnHealthChanged -= OnHealthChanged;
		}

		private void Start()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			UpdateColor();
			UpdateFillRatio();
		}

		void OnValidate()
		{
			// PATCH: Obort. GameSettings.Instance can't be obtained if the scene is not loaded.
			// TODO: I need a reliable way to load the settings at any moment.
			if (!gameObject.scene.isLoaded)
			{
				return;
			}

			SetColor(_team);
			SetFillRatio(_healthRatio);
		}

		private void Update()
		{

			if (!_character.Health.IsAlmostEqual(_lastHealth))
			{
				UpdateFillRatio();
				PlayHighlightEffect();
			}
		}

		private void OnHealthChanged(float prevHealth, float newHealth)
		{
			if (!_character.Health.IsAlmostEqual(_lastHealth))
			{
				return;
			}

			UpdateFillRatio();
			PlayHighlightEffect();
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
				healthBarColors = UISettings.EnemyHealthBarColors;
			}
			else
			{
				healthBarColors = UISettings.AllyHealthBarColors;
			}

			_healthBarFrame.color = healthBarColors.FrameColor;
			_healthBarFill.color = healthBarColors.BarColor;
		}


		private void UpdateFillRatio()
		{
			if (_character == null)
			{
				return;
			}

			SetFillRatio(_character.Health / _character.MaxHealth);

			_lastHealth = _character.Health;
		}

		private void SetFillRatio(float healthRatio)
		{
			_healthBarFill.fillAmount = healthRatio;
		}


		private void PlayHighlightEffect()
		{
			// TODO: Implement highlight effect
		}
	}
}
