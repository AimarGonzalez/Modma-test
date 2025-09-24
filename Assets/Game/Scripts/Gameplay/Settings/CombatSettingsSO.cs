using AG.Core;
using AG.Core.UI;
using UnityEngine;
using System;
using AG.Gameplay.Characters;
using System.Linq;

namespace AG.Gameplay.Settings
{
	[CreateAssetMenu(fileName = "CombatSettings", menuName = "AG/Settings/CombatSettings", order = ToolConstants.SettingsMenuOrder)]
	public class CombatSettingsSO : ScriptableObject
	{

		[Serializable]
		private class TeamTargetLayers
		{
			public LayerMask TargetLayer;

			public Team Team;
		}

		[Serializable]
		public class VFXSettings
		{
			[SerializeField]
			private PooledVFX _hitVFX;

			public PooledVFX HitVFX => _hitVFX;
		}

		// ----- Inspector fields -----

		[SerializeField]
		private TeamTargetLayers[] _projectileTargetLayers;

		[SerializeField]
		private LayerMask _wallsLayer;

		[SerializeField]
		private VFXSettings _vfxSettings;


		// ----- Public getters -----
		public LayerMask WallsLayer => _wallsLayer;

		public LayerMask GetProjectileTargetLayer(Team team)
		{
			return _projectileTargetLayers.FirstOrDefault(x => x.Team == team)?.TargetLayer ?? 0;
		}

		


		public VFXSettings VFX => _vfxSettings;
	}
}