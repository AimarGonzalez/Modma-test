using AG.Core;
using AG.Core.UI;
using AG.Gameplay.Systems;
using JetBrains.Annotations;
using AG.Gameplay.Levels;
using SharedLib.ExtensionMethods;
using Sirenix.OdinInspector;
using System;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Combat
{
	[Flags]
	public enum AppState
	{
		None = 0,
		Welcome = 1 << 0,
		BattleIntro = 1 << 1,
		Battle = 1 << 2,
		BattlePaused = 1 << 3,
		LevelLost = 1 << 4,
		LevelComplete = 1 << 5,
		Any = Welcome | BattleIntro | Battle | BattlePaused | LevelLost | LevelComplete
	}

	public class ApplicationFlow : MonoBehaviour, IGUIDrawer
	{
		// ------------- Dependencies -------------
		[Inject] private ApplicationEvents _appEvents;
		[Inject] private ApplicationView _appView;
		[Inject] private TimeController _timeController;
		[Inject] private GameplayFlow _gameplayFlow;
		[Inject] private LevelController _levelController;


		// ------------- Private fields -------------
		[ShowInInspector, ReadOnly]
		private AppState _appState = AppState.None;
		// ------------- Public properties -------------

		public bool HasActiveBattle => _appState == AppState.Battle;

		private void Start()
		{
			_appView.SetupUIAtGameStart();

			SetState(AppState.Welcome);
		}

		private void SetupNewBattle()
		{
			_gameplayFlow.SetupNewBattle();
		}

		private async Task StartBattle()
		{

			SetState(AppState.BattleIntro); // Not implemented

			// await intro
			// - show banner with GOAL (2s)
			// - spawn some enemies
			await Task.Yield();

			SetState(AppState.Battle);
		}

		private void PauseBattle()
		{
			SetState(AppState.BattlePaused);
		}

		private void ResumeBattle()
		{
			SetState(AppState.Battle);
		}

		private void RestartApp()
		{
			SetState(AppState.Welcome);
		}

		public void Update()
		{
			if (_appState == AppState.Battle)
			{
				_gameplayFlow.Update();
			}
		}

		private void SetState(AppState newAppState)
		{
			if (_appState == newAppState)
			{
				return;
			}

			AppState oldAppState = _appState;
			_appState = newAppState;

			ProcessTransition(oldAppState, newAppState);
		}

		private void ProcessTransition(AppState oldAppState, AppState newAppState)
		{
			_appEvents.TriggerAppStateChanged(oldAppState, newAppState);

			switch (oldAppState)
			{
				case AppState.Battle:
					break;

				case AppState.BattlePaused:
					_timeController.Pause(false);
					break;
			}

			switch (newAppState)
			{
				case AppState.Welcome:
					SetupNewBattle();
					break;

				case AppState.BattleIntro:
					//TODO
					break;

				case AppState.Battle:
					if (oldAppState == AppState.BattleIntro)
					{
						_levelController.StartLevel();
						_gameplayFlow.StartBattle();
					}
					else
					{
						_gameplayFlow.ResumeBattle();
					}
					break;

				case AppState.BattlePaused:
					_timeController.Pause(true);
					_gameplayFlow.PauseBattle();
					break;

				case AppState.LevelLost:
					//RestartApp();
					break;

				case AppState.LevelComplete:
					//RestartApp();
					break;
			}

			_appView.PlayViewTransition(oldAppState, newAppState).RunAsync();
		}

		void IGUIDrawer.DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Timer: {_gameplayFlow.Timer}");

			GUILayout.BeginHorizontal();

			GUI.enabled = !HasActiveBattle;
			if (GUILayout.Button("Resume"))
			{
				ResumeBattle();
			}

			GUI.enabled = HasActiveBattle;
			if (GUILayout.Button("Pause"))
			{
				PauseBattle();
			}

			GUI.enabled = _gameplayFlow != null;
			if (GUILayout.Button("Reset"))
			{
				RestartApp();
			}

			GUILayout.EndHorizontal();
		}

		// ------------- Input from UI (UnityEvents) ----------------

		[UsedImplicitly]
		public void UI_OnStartBattleButton()
		{
			StartBattle().RunAsync();
		}

		[UsedImplicitly]
		public void UI_OnPauseButton()
		{
			PauseBattle();
		}

		[UsedImplicitly]
		public void UI_OnResumeButton()
		{
			ResumeBattle();
		}

		[UsedImplicitly]
		public void UI_OnRestartAppButon()
		{
			RestartApp();
		}
	}
}