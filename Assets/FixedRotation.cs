using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour
{
    void Start()
    {
        string txt = GetComponent<TextMesh>().text;

        float whatev = txt.Length * -.24f;

        print(transform.position);

        transform.Translate(new Vector3(-whatev, 0f, 0f), Space.World);

        print(transform.position);
    }
    void Update()
    {
        transform.eulerAngles = new Vector3(0f, -180f, 0f);
    }
}
