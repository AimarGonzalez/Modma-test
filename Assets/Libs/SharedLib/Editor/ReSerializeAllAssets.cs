

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SharedLib.Utils
{
	public class ReSerializeAllAssets : MonoBehaviour
	{
		/// <summary>
		/// Sets the asset dirty and saves it, this forces Unity to serialize the selected asset(s).
		/// Useful if you want to get rid of [FormerlySerializedAs] attributes
		/// </summary>
		[MenuItem("/Assets/Force reserialize", false, 1000)]
		[MenuItem("AG/Tools/Force reserialize selected assets", false, 1000)]
		public static void ReserializeSelectedAssets()
		{
			foreach (Object obj in Selection.objects)
			{
				if (obj != null)
				{ 
					EditorUtility.SetDirty(obj);
				}
			}
			
			AssetDatabase.SaveAssets();

			LogReserializedAssetPaths(Selection.assetGUIDs);
		}
		
		[MenuItem("/Assets/Force reserialize", true)]
		private static bool ForceSaveAssetsMenuValidator()
		{
			return Selection.objects.Length > 0;
		}

		private static void LogReserializedAssetPaths(string[] selectedAssets)
		{
			List<string> existingAssetsPaths = new ();
			List<(string guid, string path)> nonExistingAssetsPaths = new ();
			foreach (string guid in selectedAssets)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				if (AssetDatabase.AssetPathExists(path))
				{
					existingAssetsPaths.Add(path);
				}
				else
				{
					nonExistingAssetsPaths.Add((guid, path));
				}
			}


			//Print logs of the reserialized paths
			foreach (string path in existingAssetsPaths)
			{
				Debug.Log($"Reserialized file: {path}");
			}

			foreach ( (string guid, string path) in nonExistingAssetsPaths)
			{
				Debug.Log($"Non existing GUIs file: {path} ({guid})");
			}
		}

		// For some reason ForceSerializeAssets insists on failing with error:
		/*
		[MenuItem("/Assets/Force reserialize (broken)", false, 1001)]
		public static void ReserializeSelectedAssets_Broken()
		{
			string[] selectedAssets = Selection.assetGUIDs;

			//Force reserialize the selected assets
			AssetDatabase.ForceReserializeAssets(selectedAssets, ForceReserializeAssetsOptions.ReserializeAssets);

			LogReserializedAssetPaths(selectedAssets);
		}
		*/

		[MenuItem("AG/Tools/Re-serialize all assets", false, 1001)]
		public static void ForceReserializeAllAssets()
		{
			//Show a confirmation dialog, than
			if (EditorUtility.DisplayDialog("Re-serialize all assets", "This is only recommended after upgrading unity version."
						+ "Are you sure you want to re-serialize all assets? This will take a while.", "Yes", "No"))
			{
				//Re-serialize all assets
				AssetDatabase.ForceReserializeAssets();
			}
		}
	}
}