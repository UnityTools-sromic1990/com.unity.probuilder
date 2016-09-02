using UnityEngine;
using UnityEditor;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using ProBuilder2.Interface;
using System.Linq;
using Parabox.STL;

namespace ProBuilder2.Actions
{
	public class ExportAsset : pb_MenuAction
	{
		public override pb_ToolbarGroup group { get { return pb_ToolbarGroup.Export; } }
		public override Texture2D icon { get { return null; } }
		public override pb_TooltipContent tooltip { get { return _tooltip; } }
		public override bool isProOnly { get { return false; } }

		static readonly pb_TooltipContent _tooltip = new pb_TooltipContent
		(
			"Export Asset",
			"Export a Unity mesh asset file."
		);

		public override bool IsEnabled()
		{
			return 	pb_Editor.instance != null &&
					selection != null &&
					selection.Length > 0;
		}

		public override bool IsHidden() { return true; }

		public override pb_ActionResult DoAction()
		{
			MakeAsset();
			return new pb_ActionResult(Status.Success, "Make Asset & Prefab");
		}

		public static void MakeAsset()
		{
			string path = "Assets";
			path = AssetDatabase.GetAssetPath(Selection.activeObject);
			Mesh meshAsset = null;

			if(path == "" || path == string.Empty) 
			{
				path = "Assets/ProBuilder Saved Assets";
			}

			if(!System.IO.Directory.Exists(path + "/ProBuilder Saved Assets"))
			{
				AssetDatabase.CreateFolder("Assets", "ProBuilder Saved Assets");
				AssetDatabase.Refresh();
			}

			foreach(pb_Object pb in pbUtil.GetComponents<pb_Object>(Selection.transforms))
			{
				string meshPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + pb.name + ".asset");

				pb.ToMesh();
				pb.Refresh();
				pb.Optimize();

				AssetDatabase.CreateAsset(pb.msh, meshPath);
				
				pb.MakeUnique();

				meshAsset = (Mesh) AssetDatabase.LoadAssetAtPath(meshPath, typeof(Mesh));

				GameObject go = new GameObject();
				go.AddComponent<MeshFilter>().sharedMesh = meshAsset;
				go.AddComponent<MeshRenderer>().sharedMaterials = pb.gameObject.GetComponent<MeshRenderer>().sharedMaterials;

				PrefabUtility.CreatePrefab(AssetDatabase.GenerateUniqueAssetPath(path + "/" + pb.name + ".prefab"), go, ReplacePrefabOptions.Default);
				GameObject.DestroyImmediate(go);
			}

			AssetDatabase.Refresh();

			Selection.activeObject = meshAsset;
		}
	}
}
