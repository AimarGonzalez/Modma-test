using AG.Gameplay.Characters;
using AG.Gameplay.Systems;
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
			FetchEntities();

			CreateSpawnPointsCatalog();
		}

		private void CreateSpawnPointsCatalog()
		{
			foreach (Transform spawnPointSet in _spawnPointsContainer)
			{
				_spawnPointsCatalog[spawnPointSet.childCount] = spawnPointSet.GetComponentsInChildren<Transform>(includeInactive: true);
			}
		}

		private void FetchEntities()
		{
			Character[] characters = _charactersContainer.GetComponentsInChildren<Character>(includeInactive: true);
			_player = characters.FirstOrDefault(character => character.IsPlayer);
			_enemies = characters.Where(character => character.IsEnemy).ToList();
		}

		public Character GetClosestEnemy(Vector3 position)
		{
			return _enemies.OrderBy(enemy => Vector3.Distance(enemy.RootTransform.position, position)).FirstOrDefault();
		}

		public Transform[] GetSpawnPoints(int numSpawnPoints)
		{
			if(_spawnPointsCatalog.TryGetValue(numSpawnPoints, out Transform[] spawnPoints))
			{
				return spawnPoints;
			}

			return null;
		}
	}
}
