using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

public class BarFillDisplay : MonoBehaviour
{


	public MonoBehaviour script;
	public int valueFieldSelection, maxFieldSelection;
	public string[] fieldNames;
	
	string valueFieldName, maxFieldName;
	
	Image image;
	FieldInfo valueField,maxField;
	float value,max;
	
	void Start()
	{
		image = GetComponent<Image>();
		//Debug.Log ("Script type " + script.GetType().ToString());
		/*
		if(script.GetType().Equals(typeof(GameObject))) {
			script = ((GameObject)script).GetComponent<MonoBehaviour>();
		}
		*/
		valueFieldName = fieldNames[valueFieldSelection];
		maxFieldName = fieldNames[maxFieldSelection];
		valueField = script.GetType().GetField(valueFieldName);
		maxField = script.GetType().GetField(maxFieldName);
		//Debug.Log (this.name + " reading from " + field.Name);
	}
	
	void Update()
	{
		if(valueField != null && maxField != null) {
			value = (float)valueField.GetValue(script);
			max = (float)maxField.GetValue(script);
			image.fillAmount = value/max;
		}
	}



}

