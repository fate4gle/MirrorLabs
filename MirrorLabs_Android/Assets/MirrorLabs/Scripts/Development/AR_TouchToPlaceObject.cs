using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class AR_TouchToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    private Pose PlacementPose;
    private Pose IndicatorPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool isPlacementActive = true;
    public TMP_Text DebugText;
    public GameObject ARSessionObject;
   

    void Start()
    {
        
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (isPlacementActive)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }

        // Inlcude this for placing the robot whenever the touchscreen is used (Even  if only the UI elements are supposed to be used it will trigger)
        /*
        if(placementPoseIsValid && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)//&& EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
        {
            ARSessionObject.transform.rotation = PlacementPose.rotation;
            ARSessionObject.transform.position = PlacementPose.position;
        }
        */
    }
    
    public void PlaceRobot()
    {
        if (isPlacementActive)
        {
            ARSessionObject.transform.rotation = PlacementPose.rotation;
            ARSessionObject.transform.position = PlacementPose.position;
            placementIndicator.SetActive(false);
            isPlacementActive = false;
        }
        else
        {
            isPlacementActive = true;
        }

    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(IndicatorPose.position, IndicatorPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        try
        {

            var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;
            DebugText.text = "found hit? " + placementPoseIsValid.ToString() + hits[0].pose.ToString();
            if (placementPoseIsValid)
            {
                
                Quaternion placementRotation = Quaternion.Euler(hits[0].pose.rotation.eulerAngles.x + 90,
                                                                hits[0].pose.rotation.eulerAngles.y, 
                                                                hits[0].pose.rotation.eulerAngles.z);
                                                                
                IndicatorPose.position = hits[0].pose.position;
                IndicatorPose.rotation = placementRotation;

                PlacementPose = hits[0].pose;
            }
        }
        catch(Exception e)
        {
            DebugText.text = e.ToString();
        }
    }
}