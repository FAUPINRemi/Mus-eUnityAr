using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARreticle : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject reticle;

    private Pose hitPose;
    private bool isValid;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            isValid = true;
            hitPose = hits[0].pose;

            if (!reticle.activeSelf)
                reticle.SetActive(true);

            reticle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
        }
        else
        {
            isValid = false;
            if (reticle.activeSelf)
                reticle.SetActive(false);
        }
    }

    public bool HasValidHit()
    {
        return isValid;
    }

    public Pose GetHitPose()
    {
        return hitPose;
    }
}