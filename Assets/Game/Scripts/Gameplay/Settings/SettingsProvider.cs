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
		private CombatSettings _combatSettings;

		[SerializeField]
		private CheatSettingsSO _cheatSettings;

		public UISettings UISettings => _uiSettings;
		public CombatSettings CombatSettings => _combatSettings;
		public CheatSettingsSO CheatSettings => _cheatSettings;
	}
}
