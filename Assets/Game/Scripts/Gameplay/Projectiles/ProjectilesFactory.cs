using AG.Core.Pool;
using AG.Gameplay.Combat;
using UnityEngine;
using VContainer;

namespace Modma.Game.Scripts.Gameplay.Projectiles
{
	public class ProjectileFactory : MonoBehaviour
	{

		// ------------ Dependencies ----------------

		[Inject] private GameObjectPoolService _poolService;
		[Inject] private GameplayWorld _gameplayWorld;
		
		// ------------ Private fields ----------------

		public ProjectileController BuildProjectile(Transform sourceAnchor, GameObject prefab)
		{
			ProjectileController projectile = _poolService.Get<ProjectileController>(
				prefab,
				_gameplayWorld.ProjectilesContainer,
				sourceAnchor.position,
				Quaternion.identity,
				inWorldSpace: true
			);
			return projectile;
		}
	}
}