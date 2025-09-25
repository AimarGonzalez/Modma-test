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
		private class TeamDamageLayers
		{
			public Team Team;
			public string DamageLayer;
		}

		[Serializable]
		public class VFXSettings
		{
			[SerializeField]
			private PooledVFX _hitVFX;

			public PooledVFX HitVFX => _hitVFX;
		}

		// ----- Inspector fields -----

		[SerializeField] private LayerMask _wallsLayer;
		[SerializeField] private LayerMask _characterLayers;
		[SerializeField] private TeamDamageLayers[] _projectileDamageLayers;
		[SerializeField] private VFXSettings _vfxSettings;


		// ----- Public getters -----
		public LayerMask WallsLayer => _wallsLayer;

		public LayerMask CharacterLayers => _characterLayers;

		public LayerMask GetProjectileDamageLayer(Character sourceCharacter)
		{
			return GetProjectileDamageLayer(sourceCharacter.Team);
		}
		
		public int GetProjectileDamageLayer(Team sourceTeam)
		{
			string damageLayer = _projectileDamageLayers.FirstOrDefault(x => x.Team == sourceTeam)?.DamageLayer ?? string.Empty;
			if (string.IsNullOrEmpty(damageLayer))
			{
				return 0;
			}
			return LayerMask.NameToLayer(damageLayer);
		}


		public VFXSettings VFX => _vfxSettings;
	}
}