using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingJoystick))]
public class FloatingJoystickEditor : JoystickEditor
{
	private SerializedProperty hideWhenActive;
	protected override void OnEnable()
	{
		base.OnEnable();
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
	}
}