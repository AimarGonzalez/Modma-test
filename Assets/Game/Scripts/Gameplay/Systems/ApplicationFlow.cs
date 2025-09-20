using AG.Core.UI;
using AG.Gameplay.Systems;
using System;
using UnityEngine;
using VContainer;


namespace AG.Gameplay.Combat
{
	public class ApplicationFlow : MonoBehaviour, IGUIDrawer
	{
		[Inject]
		private ArenaWorld _arenaWorld;

		[Inject]
		private ApplicationEvents _appEvents;

		[Flags]
		public enum State
		{
			Welcome = 1 << 0,
			BattleIntro = 1 << 1,
			Battle = 1 << 2,
			BattlePaused = 1 << 3,
			Any = Welcome | BattleIntro | Battle | BattlePaused
		}

		private State _state;

		private GameplayFlow _gameplay;

		public bool HasActiveBattle => _state == State.Battle;

		private void Start()
		{
			InitializeBattle();
		}

		private void InitializeBattle()
		{

			_gameplay = new GameplayFlow();

			ResetBattle();
		}

		public void StartBattle()
		{
			_gameplay.StartBattle();

			SetState(State.Battle);
			_appEvents.TriggerBattleStarted(_gameplay);
		}

		public void PauseBattle()
		{
			_gameplay.PauseBattle();
			
			SetState(State.BattlePaused);
			_appEvents.TriggerBattlePaused(_gameplay);
		}

		public void ResetBattle()
		{
			_gameplay.ResetBattle();

			SetState(State.BattleIntro);
			_appEvents.TriggerBattleCreated(_gameplay);
		}

		public void Update()
		{
			if (_state == State.Battle)
			{
				_gameplay.Update();
			}
		}

		private void SetState(State newState)
		{
			if (_state == newState)
			{
				return;
			}

			State oldState = _state;
			_state = newState;
			_appEvents.TriggerGameStateChanged(oldState, newState);
		}

		public void DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Timer: {_gameplay.Timer}");

			GUILayout.BeginHorizontal();


			GUI.enabled = !HasActiveBattle;
			if (GUILayout.Button("Start"))
			{
				StartBattle();
			}

			GUI.enabled = HasActiveBattle;
			if (GUILayout.Button("Pause"))
			{
				PauseBattle();
			}

			GUI.enabled = _gameplay != null;
			if (GUILayout.Button("Reset"))
			{
				ResetBattle();
			}

			GUILayout.EndHorizontal();
		}
	}
}