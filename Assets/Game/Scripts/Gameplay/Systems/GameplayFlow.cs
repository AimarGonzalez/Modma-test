using AG.Core.Pool;
using AG.Gameplay.Characters;
using UnityEngine;
using VContainer;


namespace AG.Gameplay.Combat
{
	// IMPROVE: Split between BattleTools and Battle
	public class GameplayFlow
	{

		// ------------- Dependencies -------------

		[Inject] private GameplayWorld _world;

		// ------------- Private fields -------------
		private float _currentTime;
		private bool _isBattleActive;

		// ------------- Public properties -------------
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

		public void SetupNewBattle()
		{
			// Reset in case scene is not in a correct state
			ResetBattle();

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
		}

		public void ResumeBattle()
		{
			_isBattleActive = true;
		}

		public void ResetBattle()
		{
			_isBattleActive = false;

			//Dispose enemies
			foreach (Character enemy in _world.Enemies)
			{
				enemy.ReleaseToPool();
			}

			//Reset player
			_world.Player.RootTransform.localPosition = Vector3.zero;
			_world.Player.RootTransform.localRotation = Quaternion.identity;
			_world.Player.Cinematic();

			//Reset timer
			_currentTime = 0f;
		}
	}
}