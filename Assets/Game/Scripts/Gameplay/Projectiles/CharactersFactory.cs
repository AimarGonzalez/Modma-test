using AG.Core.Pool;
using AG.Gameplay.Characters;
using AG.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Projectiles
{
	public class CharactersFactory : MonoBehaviour
	{

		// ------------ Dependencies ----------------

		[Inject] private GameObjectPoolService _poolService;
		[Inject] private GameplayWorld _gameplayWorld;
		
		// ------------ Private fields ----------------

		public Character BuildCharacter(GameObject prefab, Vector3 position, Quaternion rotation, bool active = true)
		{
			Character character = _poolService.Get<Character>(
				prefab,
				_gameplayWorld.ProjectilesContainer,
				active,
				position,
				rotation,
				inWorldSpace: true
			);
			
			_gameplayWorld.RegisterNewCharacter(character);
			
			return character;
		}
	}
}