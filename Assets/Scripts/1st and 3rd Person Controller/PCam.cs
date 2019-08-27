using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCam : MonoBehaviour
{
    public AudioListener audioListener;
    public Camera camComponent;

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
