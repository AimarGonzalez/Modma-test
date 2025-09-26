using AG.Core.Pool;
using AG.Gameplay.Characters;
using AG.Gameplay.Levels;
using UnityEngine;
using VContainer;


namespace AG.Gameplay.Combat
{
	// IMPROVE: Split between BattleTools and Battle
	public class GameplayFlow
	{

		// ------------- Dependencies -------------

		[Inject] private GameplayWorld _world;
		[Inject] private LevelController _levelController;

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
			// Reset in case Scene is not in a correct state
			ResetBattle();

			_currentTime = 0f;
			_isBattleActive = false;
		}

		public void StartBattle()
		{
			_isBattleActive = true;
			_levelController.StartLevel();
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
			Character[] iterableEnemies = _world.Enemies.ToArray(); 
			foreach (Character enemy in iterableEnemies)
			{
				enemy.ReleaseToPool();
			}

			//Reset player
			_world.Player.RootTransform.localPosition = Vector3.zero;
			_world.Player.RootTransform.localRotation = Quaternion.identity;
			_world.Player.Reset();
			_world.Player.Cinematic();

			//Reset timer
			_currentTime = 0f;
		}
	}
}