using AG.Core;
using AG.Core.UI;
using UnityEngine;
using System;

namespace AG.Gameplay.Settings
{
	[CreateAssetMenu(fileName = "CombatSettings", menuName = "AG/Settings/CombatSettings", order = ToolConstants.SettingsMenuOrder)]
	public class CombatSettings : ScriptableObject
	{
		[Serializable]
		public class VFXSettings
		{
			[SerializeField]
			private PooledVFX _hitVFX;

			public PooledVFX HitVFX => _hitVFX;
		}

		[SerializeField]
		private VFXSettings _vfxSettings;

		public VFXSettings VFX => _vfxSettings;
	}
}