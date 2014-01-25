using UnityEngine;
using System.Collections;

public class CameraZooming : MonoBehaviour
{

    #region camTrack variables
    private GameObject camTrack;
    private float playerDistance;
    #endregion

    #region Camera variables
    //Camera motion scaling.
    [SerializeField]
    private float zoomScale, minZoomDist, trackScale;

    //Camera positioning.
    private Vector3 currentPosition;
    private Vector3 cameraOrigin = new Vector3(0f, 600f, 0f);
    #endregion

    void Start()
    {
        camTrack = GameObject.Find("CamTrack");
    }

    void Update()
    {
        playerDistance = camTrack.GetComponent<CameraTrackingDevice>().playerDist;
        transform.LookAt(camTrack.transform.position);
        transform.position = new Vector3(trackScale * camTrack.transform.position.x, trackScale * camTrack.transform.position.y, minZoomDist + zoomScale * playerDistance);
    }
}
