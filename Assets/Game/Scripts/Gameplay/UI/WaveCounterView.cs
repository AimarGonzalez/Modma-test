using AG.Gameplay.Systems;
using TMPro;
using UnityEngine;
using VContainer;

namespace Modma.Game.Scripts.Gameplay.UI
{
	public class WaveCounterView : MonoBehaviour
	{
		private TextMeshProUGUI _text;

		[Inject] private ArenaEvents _arenaEvents;

		private void Awake()
		{
			_text = GetComponent<TextMeshProUGUI>();

			_arenaEvents.OnWaveChanged += OnWaveChanged;

		}

		private void OnWaveChanged(int waveIndex, int totalWaves)
		{
			_text.text = $"WAVE {waveIndex + 1}/{totalWaves}";
		}

		private void OnDestroy()
		{
			_arenaEvents.OnWaveChanged -= OnWaveChanged;
		}
	}
}