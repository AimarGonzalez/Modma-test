using AG.Core.UI;
using System;
using UnityEngine;
using VContainer;


namespace AG.Gameplay.Combat
{
	// IMPROVE: Split between BattleTools and Battle
	public class GameplayFlow
	{
		private float _currentTime;
		private bool _isBattleActive;

		public bool IsBattleActive => _isBattleActive;
		public float Timer => _currentTime;

		public GameplayFlow()
		{
		}

		public void Update()
		{
			if (!_isBattleActive)
			{
				return;
			}

			_currentTime += Time.deltaTime;
		}

		public void ResetBattle()
		{
			_currentTime = 0f;
			_isBattleActive = false;
		}

		public void StartBattle()
		{
			_isBattleActive = true;
		}

		public void PauseBattle()
		{
			_isBattleActive = false;
			// TODO: Implement battle end logic (determine winner, show results, etc.)
		}
	}
}