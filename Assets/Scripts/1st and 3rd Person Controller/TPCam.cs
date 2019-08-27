using UnityEngine;

// This class corresponds to the 3rd person camera features.
public class TPCam : MonoBehaviour 
{
    public AudioListener audioListener;
    public Camera camComponent;

    public Transform target;
    Vector3 offset;

    private void Awake()
    {
        offset = transform.localPosition;
    }

    public void UpdateCam()
    {
    }


    public void SetZoom(float zoom)
    {
        transform.localPosition = (offset * zoom);
    }



    public void EnableCam()
    {
        camComponent.enabled = true;
        audioListener.enabled = true;
    }

    public void DisableCam()
    {
        camComponent.enabled = false;
        audioListener.enabled = false;
    }
}
