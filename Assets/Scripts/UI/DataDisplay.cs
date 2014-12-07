using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

[RequireComponent(typeof(Text))]
public class DataDisplay : MonoBehaviour 
{

	public Object script;
	public int fieldSelection;
	public string[] fieldNames;
	public static System.Func<string,string>[] formatFunctions = new System.Func<string,string>[]
	{
		None,MinutesSeconds,Multiplier
	};
	public static string[] formatFunctionNames = new string[]
	{
		"None","Minutes and Seconds","Multiplier"
	};
	public int formatSelection;
	string fieldName;

	Text text;
	FieldInfo field;
	object value;

	void Start()
	{
		text = GetComponent<Text>();
		//Debug.Log ("Script type " + script.GetType().ToString());
		if(script.GetType().Equals(typeof(GameObject))) {
			script = ((GameObject)script).GetComponent<MonoBehaviour>();
		}
		fieldName = fieldNames[fieldSelection];
		field = script.GetType().GetField(fieldName);
		//Debug.Log (this.name + " reading from " + field.Name);
	}

	void Update()
	{
		if(field != null) {
			value = field.GetValue(script);
			text.text = formatFunctions[formatSelection](value.ToString());
		}
	}

	static string None(string text)
	{
		return text;
	}

	static string MinutesSeconds(string seconds)
	{
		float intSeconds = float.Parse(seconds);
		System.TimeSpan ts = new System.TimeSpan(0,0,(int)intSeconds);
		string secs = ts.Seconds.ToString();
		if(ts.Seconds < 10)
			secs = "0" + secs;
		return ts.Minutes.ToString() + ":" + secs;
	}

	static string Multiplier(string multi)
	{
		return "x" + multi;
	}

}
