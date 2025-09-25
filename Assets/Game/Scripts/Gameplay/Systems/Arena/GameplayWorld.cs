using AG.Gameplay.Characters;
using AG.Gameplay.Systems;
using SharedLib.ExtensionMethods;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Combat
{
	[DefaultExecutionOrder(-4000)]
	public class GameplayWorld : MonoBehaviour
	{

		//----- Inspector fields ----------------
		[SerializeField, Required]
		private Transform _charactersContainer;

		[SerializeField, Required]
		private Transform _movementTargetsContainer;

		[SerializeField, Required]
		private Transform _projectilesContainer;

		[SerializeField, Required]
		private Transform _spawnPointsContainer;
		
		[SerializeField, Required]
		private MeshRenderer _walkableArea;

		//----- Dependencies ----------------

		[Inject] private ArenaEvents _arenaEvents;

		//------ Public properties ----------------
		public Transform MovementTargetsContainer => _movementTargetsContainer;
		public Transform ProjectilesContainer => _projectilesContainer;
		public Character Player => _player;
		public List<Character> Enemies => _enemies;
		public MeshRenderer WalkableArea => _walkableArea;

		// -------- Private fields
		private Character _player;
		private List<Character> _enemies = new();
		private Dictionary<int, Transform[]> _spawnPointsCatalog = new();


		private void Awake()
		{
			FetchEntitiesFromInitialScene();

			CreateSpawnPointsCatalog();
		}

		private void CreateSpawnPointsCatalog()
		{
			foreach (Transform spawnPointSet in _spawnPointsContainer)
			{
				_spawnPointsCatalog[spawnPointSet.childCount] = spawnPointSet.GetComponentsInChildren<Transform>(includeInactive: true);
			}
		}

		private void FetchEntitiesFromInitialScene()
		{
			Character[] characters = _charactersContainer.GetComponentsInChildren<Character>(includeInactive: true);
			_player = characters.FirstOrDefault(character => character.IsPlayer);
			_enemies = characters.Where(character => character.IsEnemy).ToList();
		}

		public Character GetBestAttackTarget(Vector3 position)
		{
			// Return the closest enemy that is in Combat state.
			return _enemies.Where(enemy => enemy.IsFighting)
							.OrderBy(enemy => enemy.RootTransform.position.DistanceSquared(position))
							.FirstOrDefault();
		}

		public Transform[] GetSpawnPoints(int numSpawnPoints)
		{
			if(_spawnPointsCatalog.TryGetValue(numSpawnPoints, out Transform[] spawnPoints))
			{
				return spawnPoints;
			}

			return null;
		}

		public void RegisterNewCharacter(Character character)
		{
			if (character.IsEnemy)
			{
				_enemies.Add(character);
			}
			else
			{
				// Another hero? we don't support that.
				Debug.LogError($"Character {character.Name()} already exists!");
			}
		}
	}
}
