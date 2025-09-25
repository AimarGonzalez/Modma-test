using AG.Gameplay.Characters;
using AG.Gameplay.Combat;
using AG.Gameplay.Systems;
using AG.Gameplay.Characters.Data;
using AG.Gameplay.Projectiles;
using SharedLib.ExtensionMethods;
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
		private int _currentWaveIndex = 0;

		public void StartLevel()
		{
			_arenaEvents.OnCharacterRemoved += OnCharacterRemovedHandler;

			SpawnNextWave();
		}

		private void OnCharacterRemovedHandler(Character character)
		{
			if (character.IsPlayer)
			{
				_applicationEvents.TriggerLevelLost();
				return;
			}

			if (_gameplayWorld.Enemies.Count == 0)
			{
				SpawnNextWave();
			}
		}

		private void SpawnNextWave()
		{
			Transform[] spawnPointSet = GetNextSpawnPointSet();
			if (spawnPointSet == null)
			{
				LevelComplete();
			}
			else
			{
				StartWave(spawnPointSet).RunAsync();
			}
		}

		private void LevelComplete()
		{
			_arenaEvents.OnCharacterRemoved -= OnCharacterRemovedHandler;
			_applicationEvents.TriggerLevelComplete();
		}

		public Transform[] GetNextSpawnPointSet()
		{
			Transform[] spawnPointSet = null;
			while (_currentWaveIndex < _levelDefinition.Waves.Length)
			{
				LevelDefinitionSO.Wave wave = _levelDefinition.Waves[_currentWaveIndex];
				spawnPointSet = _gameplayWorld.GetSpawnPoints(wave.CharacterDefinitions.Length);
				if (spawnPointSet != null)
				{
					return spawnPointSet;
				}
				else
				{
					_currentWaveIndex++;
				}
			}

			return spawnPointSet;
		}

		private async Awaitable StartWave(Transform[] spawnPointSet)
		{
			List<Character> newCharacters = new();
			BuildCharacters(spawnPointSet, newCharacters);

			// Play spawn animation
			foreach (Character character in newCharacters)
			{
				await Awaitable.WaitForSecondsAsync(_spawnInterval);
				character.gameObject.SetActive(true);
				character.Spawn();
			}

			await Awaitable.WaitForSecondsAsync(1);

			// Set combat state
			foreach (Character character in newCharacters)
			{
				await Awaitable.WaitForSecondsAsync(_spawnInterval);
				character.gameObject.SetActive(true);
				character.Fight();
			}
		}

		private void BuildCharacters(Transform[] spawnPointSet, List<Character> newCharacters)
		{
			LevelDefinitionSO.Wave wave = _levelDefinition.Waves[_currentWaveIndex];
			for (int i = 0; i < wave.CharacterDefinitions.Length; i++)
			{
				CharacterDefinitionSO characterDefinition = wave.CharacterDefinitions[i];
				Transform spawnPoint = spawnPointSet[i];
				Character character = _charactersFactory.BuildCharacter(characterDefinition.Prefab, spawnPoint.position, spawnPoint.rotation, active: false);
				newCharacters.Add(character);
			}
		}

	}
}