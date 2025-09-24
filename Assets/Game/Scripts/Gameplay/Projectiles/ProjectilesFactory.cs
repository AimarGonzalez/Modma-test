using AG.Core.Pool;
using AG.Gameplay.Characters;
using AG.Gameplay.Characters.MonoBehaviours.BodyLocations;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Modma.Game.Scripts.Gameplay.Projectiles
{
	public class ProjectileFactory : MonoBehaviour
	{
		[SerializeField, Required]
		private Transform _projectilesContainer;


		[Inject]
		private GameObjectPoolService _poolService;

		public ProjectileController BuildProjectile(Character attacker, GameObject prefab)
		{
			Transform sourceAnchor = attacker.Root.Get<ProjectileSourceLocation>().transform;
			ProjectileController projectile = _poolService.Get<ProjectileController>(
				prefab,
				_projectilesContainer,
				sourceAnchor.position,
				Quaternion.identity,
				inWorldSpace: true
			);
			return projectile;
		}
	}
}