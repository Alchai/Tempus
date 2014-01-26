using UnityEngine;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
    float totalTranslation = 0;

	void Start () 
    {
        totalTranslation = 0;
	}
	
	void Update () 
    {
        this.transform.Translate(new Vector3(0f, 0f, 1f * Time.deltaTime));
        totalTranslation += 1 * Time.deltaTime;
        if(Input.anyKey || totalTranslation >= 17.0f)
        {
            Application.LoadLevel("TestMenu");
        }
	}
}
