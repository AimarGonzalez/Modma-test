using AG.Core.UI;
using AG.Gameplay.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace AG.Core
{
	public class TimeController : MonoBehaviour
	{
		[SerializeField] private float _timeScale = 1f;
		[ShowInInspector, ReadOnly] private bool _paused = false;

		[SerializeField]
		private List<float> _timeScales = new() { 0.0f, 0.25f, 0.5f, 1f, 2f, 4f };
		[SerializeField]
		[LabelText("Starting index")]
		private int _timeScaleIndex = 3;

		[Serializable]
		private class GUISettings
		{
			[SerializeField] private float _width = 0.45f;
			[SerializeField] private float _height = 0.16f;
			[SerializeField] private float _marginRight = 0.015f;
			[SerializeField] private float _marginTop = 0.001f;
			[SerializeField] private float _labelWidth = 0.14f;
			[SerializeField] private float _labelMargin = 0.01f;

			public float Width => _width * Screen.width;
			public float Height => _height * Screen.height;
			public float MarginTop => _marginTop * Screen.height;
			public float MarginRight => _marginRight * Screen.width;

			public float LabeFullWidth => _labelWidth * Screen.width;

			public float LabelWidth => (_labelWidth - _labelMargin - _labelMargin) * Screen.width;
			public float LabelMargin => _labelMargin * Screen.width;
		}

		// ------------------ Dependencies ------------------

		[Inject] private CheatsStyleProvider _cheatsStyleProvider;

		[Inject] private GameSettings _gameSettings;

		// ------------------ Private fields ------------------

		[FormerlySerializedAs("_debugControlsSettings")] [SerializeField]
		private GUISettings _guiSettings;

		private GUIStyle _labelStyle;
		private GUIStyle LabelStyle => _labelStyle ??= new GUIStyle(GUI.skin.label)
		{
			alignment = TextAnchor.MiddleCenter,
			wordWrap = false,
			clipping = TextClipping.Overflow,
		};

		private void Start()
		{
			ApplyTimeScale();
		}

		private void DecreaseTimeScale()
		{
			_timeScaleIndex = Math.Max(_timeScaleIndex - 1, 0);
			_timeScale = _timeScales[_timeScaleIndex];

			ApplyTimeScale();
		}

		private void IncreaseTimeScale()
		{
			_timeScaleIndex = Math.Min(_timeScaleIndex + 1, _timeScales.Count - 1);
			_timeScale = _timeScales[_timeScaleIndex];

			ApplyTimeScale();
		}

		public void TogglePause()
		{
			_paused = !_paused;

			ApplyTimeScale();
		}

		public void Pause(bool pause)
		{
			_paused = pause;
			ApplyTimeScale();
		}


		private void ApplyTimeScale()
		{
			if (_paused)
			{
				Time.timeScale = 0;
			}
			else
			{
				Time.timeScale = _timeScale;
			}
		}

		private void OnGUI()
		{
			if (!_gameSettings.CheatSettings.ShowTimeControls)
			{
				return;
			}

			_cheatsStyleProvider.PushButtonStyle();

			float debugControlsWidth = _guiSettings.Width;
			float debugControlsHeight = _guiSettings.Height;
			float debugControlsMarginTop = _guiSettings.MarginTop;
			float debugControlsMarginRight = _guiSettings.MarginRight;
			float debugControlsX = Screen.width - debugControlsWidth - debugControlsMarginRight;
			float debugControlsY = debugControlsMarginTop;

			float buttonWidth = (debugControlsWidth - _guiSettings.LabeFullWidth - _guiSettings.LabelMargin) * 0.5f;

			GUILayout.BeginArea(new Rect(debugControlsX, debugControlsY, debugControlsWidth, debugControlsHeight), GUI.skin.box);
			GUILayout.BeginHorizontal();
			{
				GUI.enabled = _timeScaleIndex > 0;
				if (GUILayout.Button("-", GUILayoutOptions.Width(buttonWidth).ExpandHeight()))
				{
					DecreaseTimeScale();
				}

				GUI.enabled = true;
				LabelStyle.fontSize = GUIUtils.CalcAutoFontSize($"x{_timeScale:0.0#}", _guiSettings.LabelWidth);
				GUILayout.Label($"x{_timeScale:0.##}", LabelStyle, GUILayoutOptions.ExpandHeight());

				GUI.enabled = _timeScaleIndex < _timeScales.Count - 1;
				if (GUILayout.Button("+", GUILayoutOptions.Width(buttonWidth).ExpandHeight()))
				{
					IncreaseTimeScale();
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();

			_cheatsStyleProvider.PopButtonStyle();
		}
	}
} 