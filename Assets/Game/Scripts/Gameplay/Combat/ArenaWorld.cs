using AG.Gameplay.Characters;
using AG.Gameplay.Systems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Combat
{
	[DefaultExecutionOrder(-4000)]
	public class ArenaWorld : MonoBehaviour
	{

		private Character _player;
		private List<Character> _enemies = new();
		
		[Inject]
		private ArenaEvents _arenaEvents;
		
		private Transform _arenaTransform;

		private void Awake()
		{
			_arenaTransform = FindFirstObjectByType<ArenaTransform>().transform;

			FetchEntities();
			
			//_arenaEvents.OnSceneCharactersInitialized += HandleSceneCharactersInitialized;
		}


		private void OnDestroy()
		{
			//_arenaEvents.OnSceneCharactersInitialized -= HandleSceneCharactersInitialized;
		}

		private void HandleSceneCharactersInitialized()
		{
			//FetchEntities();
		}
		private void FetchEntities()
		{
			Character[] characters = _arenaTransform.GetComponentsInChildren<Character>();
			_player = characters.FirstOrDefault(character => character.IsPlayer);
			_enemies = characters.Where(character => character.IsEnemy).ToList();
		}
	}
}
