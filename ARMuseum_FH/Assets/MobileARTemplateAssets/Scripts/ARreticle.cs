using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARreticle : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject reticle;

    [Tooltip("If no `reticle` is assigned, instantiate this prefab at Start and use it.")]
    public GameObject templateReticlePrefab;

    [Tooltip("Name of a GameObject in the scene to remove automatically (e.g. 'TemplateReticle'). Leave empty to skip.")]
    public string templateInstanceNameToRemove = "TemplateReticle";

    private Pose hitPose;
    private bool isValid;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        if (reticle == null && templateReticlePrefab != null)
        {
            reticle = Instantiate(templateReticlePrefab);
            if (reticle != null)
                reticle.SetActive(false);
        }

        if (!string.IsNullOrEmpty(templateInstanceNameToRemove))
        {
            var existing = GameObject.Find(templateInstanceNameToRemove);
            if (existing != null)
                DestroyImmediate(existing);
        }
    }

    void Update()
    {
        if (raycastManager == null || reticle == null)
            return;

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