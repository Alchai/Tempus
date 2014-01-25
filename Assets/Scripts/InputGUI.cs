using UnityEngine;
using System.Collections;

public class InputGUI : MonoBehaviour 
{
	public string stringToEdit = "nothing";
	public bool CanPlay = false;
	// Use this for initialization
	void Start () 
	{
		stringToEdit = "Enter Name";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(stringToEdit != "Enter Name")
		{
			CanPlay = true;
		}
		else
		{
			CanPlay = false;
		}
	}

	void OnGUI() 
	{
		stringToEdit = GUI.TextField(new Rect(325, 140, 250, 45), stringToEdit, 25);
	}
}
