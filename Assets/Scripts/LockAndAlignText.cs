using UnityEngine;
using System.Collections;

public class LockAndAlignText : MonoBehaviour 
{
	private GameObject StringToEdit;
	private int StringLength; 
	private Vector3 ParentPos;
	private Vector3 NewEuler;
	// Use this for initialization
	void Start () 
	{
		StringToEdit = GetComponent<TextMesh>().gameObject;
		StringLength = GetComponent<TextMesh>().text.Length;
		ParentPos = transform.parent.gameObject.transform.position; 
		this.transform.position = new Vector3(ParentPos.x + (StringLength), ParentPos.y + 3, ParentPos.z);
		//this.transform.position.y += 2.0f;
		NewEuler = new Vector3(0f,180f,0f);

	}
	
	// Update is called once per frame
	void Update () 
	{
		ParentPos = transform.parent.gameObject.transform.position; 
		transform.eulerAngles = NewEuler;
		this.transform.position = new Vector3(ParentPos.x + (StringLength / 4.5f), ParentPos.y + 3, ParentPos.z);
		//StringToEdit = this.transform.parent.GetComponent<TextMesh>().text.Length;
	}
}
