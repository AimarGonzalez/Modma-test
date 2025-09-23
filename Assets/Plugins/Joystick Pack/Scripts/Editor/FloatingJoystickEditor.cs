using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingJoystick))]
public class FloatingJoystickEditor : JoystickEditor
{
	private SerializedProperty hideWhenNotActive;
	protected override void OnEnable()
	{
		base.OnEnable();
		hideWhenNotActive = serializedObject.FindProperty("_hideWhenNotActive");
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		/* AG: Disable automatic setup of joystick when inspector is loaded.
		if (background != null)
		{
		    RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
		    backgroundRect.anchorMax = Vector2.zero;
		    backgroundRect.anchorMin = Vector2.zero;
		    backgroundRect.pivot = center;
		}
		*/
	}

	protected override void DrawValues()
	{
		base.DrawValues();
		EditorGUILayout.PropertyField(hideWhenNotActive, new GUIContent("Hide When Not Active", "Whether the joystick should be hidden when not active."));
	}
}