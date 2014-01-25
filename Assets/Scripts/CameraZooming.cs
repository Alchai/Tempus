using UnityEngine;
using System.Collections;

public class CameraZooming : MonoBehaviour
{

    #region camTrack variables

    private GameObject camTrack;
    private float playerDistance;
    [SerializeField]
    private float camHeightBoost;

    #endregion

    #region Camera variables
    //Camera motion scaling.
    public float zoomScale = 0.35f;
    public float minZoomDist = 2.5f;
    public float trackScale = 1.0f;

    //Camera positioning.
    private Vector3 currentPosition;
    #endregion

    void Start()
    {
        camTrack = GameObject.Find("CamTrack");
    }

    void Update()
    {
        playerDistance = camTrack.GetComponent<CameraTrackingDevice>().playerDist;
        transform.LookAt(camTrack.transform.position);
        transform.position = new Vector3(trackScale * camTrack.transform.position.x, trackScale * camTrack.transform.position.y + camHeightBoost, minZoomDist + zoomScale * playerDistance);
    }
}
