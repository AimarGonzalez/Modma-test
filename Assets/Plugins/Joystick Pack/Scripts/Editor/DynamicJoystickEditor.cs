using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DynamicJoystick))]
public class DynamicJoystickEditor : JoystickEditor
{
	private SerializedProperty moveThreshold;
	private SerializedProperty hideWhenActive;
	protected override void OnEnable()
	{
		base.OnEnable();
		moveThreshold = serializedObject.FindProperty("moveThreshold");
		hideWhenActive = serializedObject.FindProperty("_hideWhenActive");
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
		EditorGUILayout.PropertyField(moveThreshold, new GUIContent("Move Threshold", "The distance away from the center input has to be before the joystick begins to move."));
		EditorGUILayout.PropertyField(hideWhenActive, new GUIContent("Hide When Not Active", "Whether the joystick should be hidden when not active."));
	}
}