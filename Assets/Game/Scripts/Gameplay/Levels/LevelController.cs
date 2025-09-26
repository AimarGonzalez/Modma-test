using AG.Gameplay.Characters;
using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using AG.Gameplay.Characters.Data;
using AG.Gameplay.Projectiles;
using SharedLib.ExtensionMethods;
using SharedLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Levels
{
	public class LevelController : MonoBehaviour
	{
		[SerializeField]
		private LevelDefinitionSO _levelDefinition;

		[SerializeField]
		private float _spawnInterval = 0.3f;

		[Inject] private GameplayWorld _gameplayWorld;
		[Inject] private ApplicationEvents _applicationEvents;
		[Inject] private ArenaEvents _arenaEvents;
		[Inject] private CharactersFactory _charactersFactory;

		// ------------- Private fields -------------
		private int _currentWaveIndex = -1;
		private LevelDefinitionSO.Wave _currentWave;

		private bool _isLevelRunning;

		private void Awake()
		{
			_arenaEvents.OnCharacterRemoved += OnCharacterRemovedHandler;
		}

		public void StartLevel()
		{
			_isLevelRunning = true;
			_currentWaveIndex = -1;
			SpawnNextWave();
		}

		private async Awaitable TransitionToNextWave()
		{
			await Awaitable.WaitForSecondsAsync(1);
			SpawnNextWave();
		}

		private void SpawnNextWave()
		{
			_currentWaveIndex++;

			_arenaEvents.TriggerWaveChanged(_currentWaveIndex, _levelDefinition.Waves.Length);

			if (_currentWaveIndex >= _levelDefinition.Waves.Length)
			{
				LevelComplete();
				return;
			}

			_currentWave = _levelDefinition.Waves[_currentWaveIndex];

			Transform[] spawnPointSet = _gameplayWorld.GetSpawnPoints(_currentWave.CharacterDefinitions.Length);
			if (spawnPointSet == null)
			{
				Debug.LogError($"Layout error: No spawn points found for wave {_currentWaveIndex}. Level Complete.");
				LevelComplete();
			}
			else
			{
				StartWave(spawnPointSet).RunAsync();
			}
		}

		private async Awaitable StartWave(Transform[] spawnPointSet)
		{
			List<Character> newCharacters = BuildCharacters(spawnPointSet, _currentWave);

			// Play spawn animation
			foreach (Character character in newCharacters)
			{
				await Awaitable.WaitForSecondsAsync(_spawnInterval);
				character.gameObject.SetActive(true);
				character.Spawn();
			}

			await Awaitables.WaitUntil(() => newCharacters.TrueForAll(c => c.IsInCinematicState));

			_gameplayWorld.Player.Fight();

			// Set combat state
			foreach (Character character in newCharacters)
			{
				await Awaitable.WaitForSecondsAsync(_spawnInterval);
				character.Fight();
			}
		}

		private List<Character> BuildCharacters(Transform[] spawnPointSet, LevelDefinitionSO.Wave wave)
		{
			List<Character> newCharacters = new();
			for (int i = 0; i < wave.CharacterDefinitions.Length; i++)
			{
				CharacterDefinitionSO characterDefinition = wave.CharacterDefinitions[i];
				Transform spawnPoint = spawnPointSet[i];
				Character character = _charactersFactory.BuildCharacter(characterDefinition.Prefab, spawnPoint.position, spawnPoint.rotation, active: false);
				newCharacters.Add(character);
			}
			return newCharacters;
		}

		private void OnCharacterRemovedHandler(Character character)
		{
			if (!_isLevelRunning)
			{
				return;
			}

			if (character.IsPlayer)
			{
				LevelLost();
				return;
			}

			if (_gameplayWorld.Enemies.Count == 0)
			{
				TransitionToNextWave().RunAsync();
			}
		}

		private void LevelLost()
		{
			_isLevelRunning = false;
			_applicationEvents.TriggerLevelLost();
		}

		private void LevelComplete()
		{
			_isLevelRunning = false;
			_applicationEvents.TriggerLevelComplete();
		}
	}
}