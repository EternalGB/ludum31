using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(DataDisplay))]
public class DataDisplayInspector : Editor
{

	FieldInfo[] fields;

	public override void OnInspectorGUI()
	{
		DataDisplay dd = (DataDisplay)target;
		GUILayout.BeginVertical();
		dd.script = EditorGUILayout.ObjectField("Target Script",dd.script,typeof(Object),true);

		if(dd.script != null) {
			if(dd.script.GetType().Equals(typeof(GameObject))) {
				dd.script = ((GameObject)dd.script).GetComponent<MonoBehaviour>();
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label("Field",GUILayout.Width(140));
			fields = dd.script.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
			dd.fieldNames = new string[fields.Length];
			for(int i = 0; i < fields.Length; i++) {
				dd.fieldNames[i] = fields[i].Name;
			}
			dd.fieldSelection = EditorGUILayout.Popup(dd.fieldSelection,dd.fieldNames);

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			GUILayout.Label ("Format",GUILayout.Width(140));
			dd.formatSelection = EditorGUILayout.Popup(dd.formatSelection,DataDisplay.formatFunctionNames);

			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

}

