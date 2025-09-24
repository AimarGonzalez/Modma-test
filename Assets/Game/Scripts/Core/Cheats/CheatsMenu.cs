using AG.Core.UI;
using AG.Gameplay.Combat;
using AG.Gameplay.Settings;
using UnityEngine;
using VContainer;

namespace AG.Core
{
	public class CheatsMenu : MonoBehaviour
	{
		// ------------------
		
		[SerializeField]
		private bool _showDebugPanel = false;

		// ------------------

		[Inject] private ApplicationFlow _battleCheats;
		[Inject] private GameSettings _gameSettings;

		private CheatsStyleProvider CheatsStyleProvider => GetComponent<CheatsStyleProvider>();

		private void OnGUI()
		{
			if (!_gameSettings.CheatSettings.ShowCheatsMenu)
			{
				return;
			}
			
			if (_showDebugPanel)
			{
				DrawCheatPanel();
			}
			else
			{
				DrawOpenButton();
			}
		}

		private void DrawCheatPanel()
		{
			CheatPanelSettings panelSettings = CheatsStyleProvider.PushPanelStyle();
			
			GUILayoutUtils.LabelWidth = panelSettings.LabelWidth * panelSettings.Width * Screen.width;
			GUILayoutUtils.LabelHeight = GUI.skin.label.CalcHeight(new GUIContent("X"), 100);

			GUILayout.BeginArea(new Rect(10, 10, panelSettings.Width * Screen.width, panelSettings.Height * Screen.height), GUI.skin.box);
			GUILayout.BeginVertical();

			DrawSubPanels();

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("CLOSE"))
			{
				_showDebugPanel = false;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		
			CheatsStyleProvider.PopPanelStyle();
		}

		private void DrawOpenButton()
		{
			CheatsStyleProvider.PushButtonStyle();
			
			Vector2 buttonSize = new Vector2(Screen.width * 0.35f, Screen.height * 0.06f);
			float margin = 10;
			GUILayout.BeginArea(new Rect(margin, margin, buttonSize.x - margin, buttonSize.y - margin), GUI.skin.box);
			GUILayout.BeginVertical();

			if (GUILayout.Button("CHEATS", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
			{
				_showDebugPanel = true;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
			
			CheatsStyleProvider.PopButtonStyle();
		}

		private void DrawSubPanels()
		{
			GUILayoutUtils.BeginVerticalBox(_battleCheats);
		}
	}
}
