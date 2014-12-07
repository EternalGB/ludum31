using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NavGrid))]
public class NavGridEditor : Editor 
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		NavGrid ng = (NavGrid)target;

		GUILayout.BeginVertical();
		if(GUILayout.Button("Make Symmetrical")) {
			ng.MakeSymmetrical();
		}
		GUILayout.EndVertical();
	}

}
