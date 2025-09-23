using AG.Core;
using AG.Core.UI;
using AG.Gameplay.Systems;
using JetBrains.Annotations;
using Modma.Game.Scripts.Gameplay.Systems;
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
		Welcome = 1 << 0,
		BattleIntro = 1 << 1,
		Battle = 1 << 2,
		BattlePaused = 1 << 3,
		Any = Welcome | BattleIntro | Battle | BattlePaused
	}

	public class ApplicationFlow : MonoBehaviour, IGUIDrawer
	{
		// ------------- Dependencies -------------
		[Inject] private ArenaWorld _arenaWorld;
		[Inject] private ApplicationEvents _appEvents;
		[Inject] private ApplicationView _appView;
		[Inject] private TimeController _timeController;


		// ------------- Private fields -------------
		[ShowInInspector, ReadOnly]
		private AppState _appState = AppState.Welcome;

		private GameplayFlow _gameplay;

		// ------------- Public properties -------------

		public bool HasActiveBattle => _appState == AppState.Battle;

		private void Start()
		{
			_gameplay = new GameplayFlow();
			_appEvents.TriggerBattleCreated(_gameplay);

			_appView.SetupUIAtGameStart();
			SetupNewBattle();
		}

		private void SetupNewBattle()
		{
			_gameplay.SetupNewBattle();
		}

		private async Task StartBattle()
		{
			//SetState(AppState.BattleIntro).RunAsync();
			
			// await Play intro
			// - show banner with GOAL (2s)
			// - spawn some enemies
			await Task.Yield();

			SetState(AppState.Battle);

			_gameplay.StartBattle();
		}

		private void PauseBattle()
		{
			_gameplay.PauseBattle();

			_timeController.Pause(true);

			SetState(AppState.BattlePaused);
		}

		private void ResumeBattle()
		{
			_gameplay.ResumeBattle();

			_timeController.Pause(false);

			SetState(AppState.Battle);
		}

		private void RestartApp()
		{
			SetState(AppState.Welcome);
			SetupNewBattle();
		}

		public void Update()
		{
			if (_appState == AppState.Battle)
			{
				_gameplay.Update();
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

			_appView.PlayViewTransition(oldAppState, newAppState).RunAsync();

			TriggerStateChangedEvents(oldAppState, newAppState);
		}

		private void TriggerStateChangedEvents(AppState oldAppState, AppState newAppState)
		{
			_appEvents.TriggerAppStateChanged(oldAppState, newAppState);

			switch (oldAppState)
			{
				case AppState.Battle:
					_appEvents.TriggerBattleEnded(_gameplay);
					break;
			}

			switch (newAppState)
			{
				case AppState.Battle:
					_appEvents.TriggerBattleStarted(_gameplay);
					break;
			}
		}

		void IGUIDrawer.DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Timer: {_gameplay.Timer}");

			GUILayout.BeginHorizontal();

			/*
			GUI.enabled = !HasActiveBattle;
			if (GUILayout.Button("Start"))
			{
				StartBattle().RunAsync();
			}
			*/

			GUI.enabled = HasActiveBattle;
			if (GUILayout.Button("Pause"))
			{
				PauseBattle();
			}

			GUI.enabled = _gameplay != null;
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