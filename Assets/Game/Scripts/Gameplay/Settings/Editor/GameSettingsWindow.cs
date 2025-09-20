using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace AG.Editor
{
	public class GameSettingsWindow : OdinMenuEditorWindow
	{
		[MenuItem("AG/Settings/Game Settings")]
		private static void OpenWindow()
		{
			GetWindow<GameSettingsWindow>("Game Settings").Show();
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			//tree.AddAssetAtPath("UI Settings", "Assets/Game/Settings/UISettings.asset");
			tree.AddAllAssetsAtPath("Settings", "Assets/Game/Settings", typeof(ScriptableObject), includeSubDirectories: true);
			return tree;
		}
	}
}