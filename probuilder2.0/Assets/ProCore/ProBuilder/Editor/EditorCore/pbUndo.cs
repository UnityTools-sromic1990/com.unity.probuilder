#define PB_DEBUG

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

namespace ProBuilder2.EditorCommon
{
	/**
	 * A wrapper around Unity Undo calls.  Used for debugging and (previously) version compatibility.
	 */
	public class pbUndo
	{
		/**
		 * Since Undo calls can potentially hang the main thread, store states when the diff
		 * will large.
		 */
		public static void RecordSelection(pb_Object pb, string msg)
		{
			if( pb.vertexCount > 256 )
				RegisterCompleteObjectUndo(pb, msg);
			else
				RecordObject(pb, msg);
		}

		/**
		 *	Tests if any pb_Object in the selection has more than 512 vertices, and if so records the entire object 
		 * 	instead of diffing the serialized object (which is very slow for large arrays).
		 */
		public static void RecordSelection(pb_Object[] pb, string msg)
		{
			if( pb.Any(x => { return x.vertexCount > 256; }) )
				RegisterCompleteObjectUndo(pb, msg);
			else
				RecordObjects(pb, msg);
		}

		/**
		 * Record an object for Undo.
		 */
		public static void RecordObject(Object obj, string msg)
		{
			#if PB_DEBUG
			if(obj is pb_Object && ((pb_Object)obj).vertexCount > 256)	
				Debug.LogWarning("RecordObject()  ->  " + ((pb_Object)obj).vertexCount);
			#endif
			Undo.RecordObject(obj, msg);
		}

		/**
		 * Record objects for Undo.
		 */
		public static void RecordObjects(Object[] objs, string msg)
		{
			if(objs == null) return;	

			#if PB_DEBUG
			foreach(pb_Object pb in objs)
			{
				if(pb is pb_Object && ((pb_Object)pb).vertexCount > 256)	
				{
					Debug.LogWarning("RecordObject()  ->  " + ((pb_Object)pb).vertexCount);
					break;
				}
			}
			#endif

			Undo.RecordObjects(objs, msg);
		}

		/**
		 * Undo.RegisterCompleteObjectUndo
		 */
		public static void RegisterCompleteObjectUndo(Object objs, string msg)
		{
			Undo.RegisterCompleteObjectUndo(objs, msg);
		}

		/**
		 * Undo.RegisterCompleteObjectUndo
		 */
		public static void RegisterCompleteObjectUndo(Object[] objs, string msg)
		{
			Undo.RegisterCompleteObjectUndo(objs, msg);
		}

		/**
		 * Record object prior to deletion.
		 */
		public static void DestroyImmediate(Object obj, string msg)
		{
			Undo.DestroyObjectImmediate(obj);
		}

		public static void RegisterCreatedObjectUndo(Object obj, string msg)
		{
			Undo.RegisterCreatedObjectUndo(obj, msg);
		}
	}
}
