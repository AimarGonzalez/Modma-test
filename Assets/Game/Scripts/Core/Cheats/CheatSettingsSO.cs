using AG.Core.UI;
using UnityEngine;

namespace AG.Core
{
	[CreateAssetMenu(fileName = "CheatSettings", menuName = "AG/Settings/CheatSettings", order = ToolConstants.SettingsMenuOrder)]
	public class CheatSettingsSO : ScriptableObject
	{
		public enum ShowMode
		{
			Never,
			OnlyInEditor,
			Always
		}

		[SerializeField] private ShowMode _showCheatsMenu = ShowMode.OnlyInEditor;
		[SerializeField] private ShowMode _showTimeControls = ShowMode.OnlyInEditor;
		[SerializeField] private ShowMode _showCharacterDebugPanel = ShowMode.OnlyInEditor;

		public bool ShowCheatsMenu => _showCheatsMenu == ShowMode.Always || (_showCheatsMenu == ShowMode.OnlyInEditor && Application.isEditor);
		public bool ShowTimeControls => _showTimeControls == ShowMode.Always || (_showTimeControls == ShowMode.OnlyInEditor && Application.isEditor);
		public bool ShowCharacterDebugPanel => _showCharacterDebugPanel == ShowMode.Always || (_showCharacterDebugPanel == ShowMode.OnlyInEditor && Application.isEditor);
	}
}