using AG.Core;
using AG.Core.Pool;
using AG.Gameplay.Combat;
using AG.Gameplay.Settings;
using AG.Gameplay.UI;
using Modma.Game.Scripts.Gameplay.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AG.Gameplay.Systems
{
	[DefaultExecutionOrder(-5000)]
	public class GameBootstrap : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			Debug.Log("Boostrap");
			builder.Register<ApplicationEvents>(Lifetime.Singleton);
			builder.RegisterEntryPoint<ArenaEvents>(Lifetime.Scoped).AsSelf();

			// Register MonoBehaviours
			builder.UseComponents(components =>
			{
				components.AddInHierarchy<TimeController>();
				components.AddInHierarchy<ApplicationFlow>();
				components.AddInHierarchy<ApplicationTransitions>();
				components.AddInHierarchy<ArenaWorld>();
				components.AddInHierarchy<CheatsStyleProvider>();
				components.AddInHierarchy<GameObjectPoolService>();
				components.AddInHierarchy<GameSettings>();
				components.AddInHierarchy<UIProvider>();
			});
		}
	}
}