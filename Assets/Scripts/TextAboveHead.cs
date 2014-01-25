using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class TextAboveHead : MonoBehaviour
{
    //When will now be then?
    private TextMesh textmesh;
    public GameObject p1, p2;

    void Start()
    {
        p1 = GameObject.Find("Client");
        p2 = GameObject.Find("Client");

        DisplayText(p1);
    }

    public void DisplayText(GameObject player)
    {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        GameObject newtext = new GameObject();
        newtext.AddComponent<TextMesh>();
        newtext.GetComponent<TextMesh>().color = Color.white;
        newtext.GetComponent<TextMesh>().font = ArialFont;
        newtext.GetComponent<TextMesh>().renderer.material = ArialFont.material;
        newtext.GetComponent<TextMesh>().text = SystemInfo.deviceName;
        newtext.transform.position = player.transform.position;
        newtext.transform.Translate(0f, 2f, 0f);
        newtext.transform.parent = player.transform;
    }
}
