using AG.Core;
using UnityEngine;

namespace AG.Gameplay.Settings
{
	[DefaultExecutionOrder(-9999)]
	public class GameSettings : MonoBehaviour
	{
		[SerializeField]
		private UISettings _uiSettings;

		[SerializeField]
		private CombatSettingsSO _combatSettingsSO;

		[SerializeField]
		private CheatSettingsSO _cheatSettings;

		public UISettings UISettings => _uiSettings;
		public CombatSettingsSO CombatSettingsSO => _combatSettingsSO;
		public CheatSettingsSO CheatSettings => _cheatSettings;
	}
}
