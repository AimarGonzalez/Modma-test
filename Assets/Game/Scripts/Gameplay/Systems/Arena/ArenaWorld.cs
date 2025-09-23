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
	public class ArenaWorld : MonoBehaviour
	{

		//----- Inspector fields ----------------
		[SerializeField, Required]
		private Transform _movementTargetsContainer;

		[SerializeField, Required]
		private Transform _projectilesContainer;

		//----- Dependencies ----------------

		private Character _player;
		private List<Character> _enemies = new();

		[Inject]
		private ArenaEvents _arenaEvents;

		private Transform _arenaTransform;

		//------ Public properties ----------------
		public Transform MovementTargetsContainer => _movementTargetsContainer;
		public Transform ProjectilesContainer => _projectilesContainer;
		public Character Player => _player;
		public List<Character> Enemies => _enemies;

		private void Awake()
		{
			_arenaTransform = FindFirstObjectByType<ArenaTransform>().transform;

			FetchEntities();
		}

		private void FetchEntities()
		{
			Character[] characters = _arenaTransform.GetComponentsInChildren<Character>();
			_player = characters.FirstOrDefault(character => character.IsPlayer);
			_enemies = characters.Where(character => character.IsEnemy).ToList();
		}
	}
}
