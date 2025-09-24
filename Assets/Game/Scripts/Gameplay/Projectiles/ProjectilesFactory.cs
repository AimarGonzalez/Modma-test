using AG.Core.Pool;
using AG.Gameplay.Characters;
using AG.Gameplay.Characters.MonoBehaviours.BodyLocations;
using AG.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace Modma.Game.Scripts.Gameplay.Projectiles
{
	public class ProjectileFactory : MonoBehaviour
	{

		// ------------ Dependencies ----------------

		[Inject] private GameObjectPoolService _poolService;
		[Inject] private ArenaWorld _arenaWorld;
		
		// ------------ Private fields ----------------

		public ProjectileController BuildProjectile(Character attacker, GameObject prefab)
		{
			Transform sourceAnchor = attacker.Root.Get<ProjectileSourceLocation>().transform;
			ProjectileController projectile = _poolService.Get<ProjectileController>(
				prefab,
				_arenaWorld.ProjectilesContainer,
				sourceAnchor.position,
				Quaternion.identity,
				inWorldSpace: true
			);
			return projectile;
		}
	}
}