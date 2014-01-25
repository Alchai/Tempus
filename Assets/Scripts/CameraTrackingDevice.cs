using UnityEngine;
using System.Collections;

public class CameraTrackingDevice : MonoBehaviour
{

    private GameObject player1;
    private GameObject player2;
    private Vector3 p1Loc;
    private Vector3 p2Loc;

    public float playerDist;
    public float cameraScaling = 1.0f;


    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        p1Loc = player1.transform.position;
        p2Loc = player2.transform.position;
        playerDist = Vector3.Distance(p1Loc, p2Loc);
        transform.position = new Vector3((p1Loc.x + p2Loc.x) * cameraScaling / 2, (p1Loc.y + p2Loc.y) / 2, (p1Loc.z + p2Loc.z) / 2);
    }
}
