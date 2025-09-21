﻿using SharedLib.ExtensionMethods;
using System.Collections.Generic;
using SharedLib.Utils;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AG.Core.UI
{
	public static class GUIUtils
	{
		
		public static float HalfScreenW => Screen.width * 0.5f;
		public static float HalfScreenH => Screen.height * 0.5f;

		public enum PanelPlacement
		{
			Bottom,
			Top,
			Left,
			Right
		}

		public struct Property
		{
			public string label;
			public string value;

			public GUIStyle labelStyle;
			public GUIStyle valueStyle;

			public Vector2 labelSize;
			public Vector2 valueSize;

			public Property(string label)
				: this(label, "")
			{
			}

			public Property(string label, int value)
				: this(label, value.ToString())
			{
			}

			public Property(string label, float value)
				: this(label, value.ToString())
			{
			}

			public Property(string label, Vector3 value)
				: this(label, $"v({value.x}, {value.y}, {value.z})")
			{
			}

			public Property(string label, Enum value)
				: this(label, value.ToString())
			{
			}

			public Property(string label, string value)
				: this(label, value, GuiStylesCatalog.DebugLabelStyle, GuiStylesCatalog.DebugTextFieldStyle)
			{
			}

			public Property(string label, string value, GUIStyle valueStyle)
				: this(label, value, GuiStylesCatalog.DebugLabelStyle, valueStyle)
			{
			}

			public Property(string label, string value, GUIStyle labelStyle, GUIStyle valueStyle)
			{
				labelStyle ??= GUI.skin.label;
				valueStyle ??= GUI.skin.textField;

				this.label = label;
				this.value = value;
				this.labelStyle = labelStyle;
				this.valueStyle = valueStyle;
				this.labelSize = CalculatedSize(label, labelStyle);
				this.valueSize = CalculatedSize(value, valueStyle);
			}
		}

		private static Vector2 CalculatedSize(string value, GUIStyle style)
		{
			if (value.IsNullOrEmpty())
			{
				return Vector2.zero;
			}

			Vector2 extraPixelToAvoidWorldWrappingGlitches = new Vector2(1f, 0f);
			return style.CalcSize(new GUIContent(value)) + GetMarginOffset(style) + extraPixelToAvoidWorldWrappingGlitches;
		}

		private static Vector2 GetMarginOffset(GUIStyle style)
		{
			return new Vector2(style.margin.horizontal, style.margin.vertical);
		}

		private static Stack<Color> _colorStack = new Stack<Color>();
		private static Stack<Color> _bgColorStack = new Stack<Color>();
		private static Stack<Color> _contentColorStack = new Stack<Color>();
		private static Stack<GUIFontSizes> _fontSizeStack = new Stack<GUIFontSizes>();
		private static Stack<GUISkin> _skinStack = new Stack<GUISkin>();

		public struct GUIFontSizes
		{
			public int label;
			public int button;
			public int textField;
			public int textArea;
			public int toggle;
			public int window;
			public int box;

			public GUIFontSizes(int defaultSize = 12)
			{
				label = defaultSize;
				button = defaultSize;
				textField = defaultSize;
				textArea = defaultSize;
				toggle = defaultSize;
				window = defaultSize;
				box = defaultSize;
				
			}

			public static GUIFontSizes FromCurrentGUI()
			{
				return new GUIFontSizes
				{
					label = GUI.skin.label.fontSize,
					button = GUI.skin.button.fontSize,
					textField = GUI.skin.textField.fontSize,
					textArea = GUI.skin.textArea.fontSize,
					toggle = GUI.skin.toggle.fontSize,
					window = GUI.skin.window.fontSize,
					box = GUI.skin.box.fontSize
				};
			}

			public void ApplyToGUI()
			{
				GUI.skin.label.fontSize = label;
				GUI.skin.button.fontSize = button;
				GUI.skin.textField.fontSize = textField;
				GUI.skin.textArea.fontSize = textArea;
				GUI.skin.toggle.fontSize = toggle;
				GUI.skin.window.fontSize = window;
				GUI.skin.box.fontSize = box;
			}
		}

		public static void PushSkin(GUISkin skin)
		{
			_skinStack.Push(GUI.skin);
			GUI.skin = skin;
		}

		public static void PopSkin()
		{
			GUI.skin = _skinStack.Pop();
		}	

		
		public static int FontSize => GUI.skin.label.fontSize;

		public static void PushFontSize(int uniformSize)
		{
			PushFontSize(new GUIFontSizes(uniformSize));
		}

		public static void PushFontSize(GUIFontSizes sizes)
		{
			_fontSizeStack.Push(GUIFontSizes.FromCurrentGUI());
			sizes.ApplyToGUI();
		}

		public static void PopFontSize()
		{
			if (_fontSizeStack.Count > 0)
			{
				_fontSizeStack.Pop().ApplyToGUI();
			}
		}

		public static int CalcAutoFontSize(string text, float availableWidth)
		{
			GUIContent content = new(text);
			float labelWidth = GUI.skin.label.CalcSize(content).x;
			float scaledFontSize = FontSize * availableWidth / labelWidth;
			return (int)scaledFontSize;
		}

		public static void PushAutoFontSize(string text, float availableWidth)
		{
			PushFontSize(CalcAutoFontSize(text, availableWidth));
		}

		public static void PushColor(Color col)
		{
			_colorStack.Push(GUI.color);
			GUI.color = col;
		}

		public static void PopColor()
		{
			GUI.color = _colorStack.Pop();
		}

		public static void PushBackgroundColor(Color col)
		{
			_bgColorStack.Push(GUI.color);
			GUI.backgroundColor = col;
		}

		public static void PopBackgroundColor()
		{
			GUI.backgroundColor = _bgColorStack.Pop();
		}

		public static void PushContentColor(Color col)
		{
			_contentColorStack.Push(GUI.contentColor);
			GUI.contentColor = col;
		}

		public static void PopContentColor()
		{
			GUI.contentColor = _contentColorStack.Pop();
		}

		public static void DrawTextField(int index,
			Property property,
			Rect rect,
			GUIStyle panelStyle,
			float labelWidth,
			float valueWidth = 0f
			)
		{

			float rowHeigh = Mathf.Max(property.labelSize.y, property.valueSize.y);
			labelWidth = labelWidth > 0f ? labelWidth : property.labelSize.x;
			valueWidth = valueWidth > 0f ? valueWidth : property.valueSize.x;

			if (property.label.IsNotEmpty())
			{
				GUI.Label(
					new Rect(
					rect.xMin + panelStyle.padding.left + property.labelStyle.margin.left, // + (rowHeigh - property.labelSize.y) * 0.5f,
					rect.yMin + panelStyle.padding.top + property.labelStyle.margin.top + index * rowHeigh,
					labelWidth - property.labelStyle.margin.horizontal,
					rowHeigh - property.labelStyle.margin.vertical),
					property.label,
					property.labelStyle);
			}

			if (property.value.IsNotEmpty())
			{
				GUI.TextField(
					new Rect(
						rect.xMin + panelStyle.padding.left + labelWidth + property.valueStyle.margin.left,
						rect.yMin + panelStyle.padding.top + property.valueStyle.margin.top + index * rowHeigh,
						valueWidth - property.valueStyle.margin.horizontal,
						rowHeigh - property.valueStyle.margin.vertical),
					property.value,
					property.valueStyle);
			}
		}

		public static (Vector2 size, float labelWidth, float valueWidth) CalcPanelSize(GUIStyle panelStyle, List<Property> properties)
		{
			panelStyle ??= GUI.skin.box;

			float maxRowHeight = 0f;
			float maxLabelWidth = 0f;
			float maxValueWidth = 0f;

			foreach (var property in properties)
			{
				maxRowHeight = Mathf.Max(maxRowHeight, Mathf.Max(property.labelSize.y, property.valueSize.y));

				maxLabelWidth = Mathf.Max(maxLabelWidth, property.labelSize.x);

				maxValueWidth = Mathf.Max(maxValueWidth, property.valueSize.x);
			}

			Vector2 size = new Vector2(maxLabelWidth + maxValueWidth + panelStyle.padding.horizontal, properties.Count * maxRowHeight + panelStyle.padding.vertical);
			return (size, maxLabelWidth, maxValueWidth);
		}

		public static Vector3 CalcPanelPosition(Transform transform, Vector2 size, PanelPlacement panelPosition, float margin = 0f)
		{
			const float CAMERA_ANGLE = 55f;
			float TAN_CAMERA_ANGLE = (float)Math.Tan((90f - CAMERA_ANGLE) * MathConst.Deg2Rad);
			var characterBounds = MeshUtils.GetBoundingBox(transform);

			float objectDepth = characterBounds.size.y;
			float objectHeight = characterBounds.size.z;
			float objectWidth = characterBounds.size.x;
			Vector2 screenOffset = Vector2.zero;
			Vector3 worldOffset = Vector3.zero;

			switch (panelPosition)
			{
				case PanelPlacement.Top:
					screenOffset = Vector2.down * (size.y * 0.5f + margin);
					worldOffset = Vector3.up * (objectDepth * 0.5f + objectHeight * TAN_CAMERA_ANGLE);
					break;
				case PanelPlacement.Bottom:
					screenOffset = Vector2.up * (size.y * 0.5f + margin);
					worldOffset = Vector3.down * objectDepth * 0.5f;
					break;
				case PanelPlacement.Left:
					screenOffset = Vector2.left * (size.x * 0.5f + margin);
					worldOffset = Vector3.left * objectWidth * 0.5f;
					break;
				case PanelPlacement.Right:
					screenOffset = Vector2.right * (size.x * 0.5f + margin);
					worldOffset = Vector3.right * objectWidth * 0.5f;
					break;
			}

			Vector3 worldPosition = transform.position + worldOffset;
			Vector2 guiPoint = WorldToGUIPoint(Camera.main, worldPosition);
			Vector3 labelPosition = guiPoint + screenOffset;
			return labelPosition;
		}

		public static Vector3 CalcPanelPositionOnUI(RectTransform target, Vector2 panelSize, PanelPlacement panelPlacement, float margin = 0f)
		{
			Vector2 panelOffset = Vector2.zero;
			Vector2 targetOffset = Vector2.zero;

			Rect rect = target.rect;
			float targetHeight = rect.height * 0.5f;
			float targetWidth = rect.width * 0.5f;

			switch (panelPlacement)
			{
				case PanelPlacement.Top:
					panelOffset = Vector2.up * (panelSize.y * 0.5f + margin);
					targetOffset = Vector2.up * targetHeight;
					break;
				case PanelPlacement.Bottom:
					panelOffset = Vector2.down * (panelSize.y * 0.5f + margin);
					targetOffset = Vector2.down * targetHeight;
					break;
				case PanelPlacement.Left:
					panelOffset = Vector2.left * (panelSize.x * 0.5f + margin);
					targetOffset = Vector2.left * targetWidth;
					break;
				case PanelPlacement.Right:
					panelOffset = Vector2.right * (panelSize.x * 0.5f + margin);
					targetOffset = Vector2.right * targetWidth;
					break;
			}

			Vector2 targetPosition = target.position;
			Vector2 screenPosition = targetPosition + targetOffset + panelOffset;
			Vector3 guiPoint = ScreenToGUIPoint(Camera.main, screenPosition);
			return guiPoint;
		}

		public static void DrawDebugPanel(List<Property> properties, Transform target, PanelPlacement panelPlacement, float margin = 0f, Action onClose = null)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = CalcPanelPosition(target, panelSize, panelPlacement, margin);

			DrawDebugPanel(properties, panelPosition, panelSize, panelStyle, labelWidth, valueWidth, onClose);
		}

		public static void DrawDebugPanel(List<Property> properties, RectTransform target, PanelPlacement panelPlacement, float margin = 0f, Action onClose = null)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth, float valueWidth) = CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = CalcPanelPositionOnUI(target, panelSize, panelPlacement, margin);

			DrawDebugPanel(properties, panelPosition, panelSize, panelStyle, labelWidth, valueWidth, onClose);
		}

		private static void DrawDebugPanel(List<Property> properties, Vector3 panelPosition, Vector2 panelSize, GUIStyle style, float labelWidth, float valueWidth, Action onClose)
		{
			style ??= GUI.skin.box;

			// Create rect centered on the panel's position
			Rect rect = new Rect(panelPosition.x - panelSize.x * 0.5f, panelPosition.y - panelSize.y * 0.5f, panelSize.x, panelSize.y);

			GUI.Box(rect, GUIContent.none, style);

			for (int i = 0; i < properties.Count; i++)
			{
				DrawTextField(i, properties[i], rect, style, labelWidth, valueWidth);
			}

			if (onClose != null)
			{
				// Get size of the button
				GUIContent closeButtonContent = new GUIContent("X");
				Vector2 buttonSize = GUI.skin.button.CalcSize(closeButtonContent);
				bool pressed = GUI.Button(new Rect(rect.x + rect.width + 2, rect.y, buttonSize.x, buttonSize.y), closeButtonContent);
				if (pressed)
				{
					onClose?.Invoke();
				}
			}
		}

		public static Vector3 ScreenToGUIPoint(Camera camera, Vector3 screenPoint)
		{
			screenPoint.y = camera.pixelHeight - screenPoint.y;
			Vector3 guiPoint = PixelsToPoints((Vector2)screenPoint);
			return guiPoint;
		}

		public static Vector3 WorldToGUIPoint(Camera camera, Vector3 worldPoint)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldPoint);
			vector.y = (float)camera.pixelHeight - vector.y;
			Vector3 guiPoint = PixelsToPoints(vector);
			return guiPoint;
		}
		
		public static Vector3 PixelsToPoints(Vector3 position)
		{
#if UNITY_EDITOR
			float num = 1f / EditorGUIUtility.pixelsPerPoint;
#else
			float num = 1f;
#endif
			position.x *= num;
			position.y *= num;
			return position;
		}

	}
}