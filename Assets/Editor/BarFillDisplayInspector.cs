using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(BarFillDisplay))]
public class BarFillDisplayInspector : Editor
{

	FieldInfo[] fields;
	MonoBehaviour[] scripts;

	public override void OnInspectorGUI()
	{
		BarFillDisplay bf = (BarFillDisplay)target;
		GUILayout.BeginVertical();
		bf.script = (MonoBehaviour)EditorGUILayout.ObjectField("Target Script",bf.script,typeof(MonoBehaviour),true);
		
		if(bf.script != null) {
			/*
			if(bf.script.GetType().Equals(typeof(GameObject))) {
				bf.script = ((GameObject)bf.script).GetComponent<MonoBehaviour>();
			}
			*/
			GUILayout.BeginHorizontal();
			fields = bf.script.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			bf.fieldNames = new string[fields.Length];
			for(int i = 0; i < fields.Length; i++) {
				bf.fieldNames[i] = fields[i].Name;
			}
			
			GUILayout.Label("Value Field",GUILayout.Width(100));
			bf.valueFieldSelection = EditorGUILayout.Popup(bf.valueFieldSelection,bf.fieldNames);
			
			GUILayout.Label("Max Field",GUILayout.Width(100));
			bf.maxFieldSelection = EditorGUILayout.Popup(bf.maxFieldSelection,bf.fieldNames);
			
			GUILayout.EndHorizontal();
			
		}


		GUILayout.EndVertical();
	}

}

