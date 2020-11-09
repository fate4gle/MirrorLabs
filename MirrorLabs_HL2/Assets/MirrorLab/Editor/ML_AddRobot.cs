/*
© TU Delft, 2020
Author: Jonas S.I. Rieder (j.s.i.rieder@tudelft.nl)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using UnityEngine;
using UnityEditor;
using RosSharp;
using RosSharp.RosBridgeClient;
using RosSharp.Urdf;


using MirrorLabs;

namespace MirrorLabs
{
    public class ML_AddRobot : EditorWindow
    {
        private static string windowTitle = "MirrorLabs Add Robot";
        private string introText = "Please configure a new robot and click on the Button to generate it!";
        private string introText_con = "The ros connetor and robot will be added automatically.";
        private string introText_last= "Please change the name for each new robot individually";
        
        //Robot Setup
        public GameObject robotPrefab;
        public string robotName;
        Editor robotPrefabPreview;
        private GameObject robot;
        private GameObject robotObj;
        private GameObject robotParentObject;

        //ROS# Setup
        private bool includeROSSharp = true;
        private bool isJointSubscribed = false;
        private bool isOdomSubscribed = false;
        private bool isJointPublisher = false;
        private bool isPoseStamp = false;
        public GameObject rosConnectorObj;
        private GameObject rosParentObject;


        [MenuItem("MirrorLabs/Add Robot")]
        public static void ShowWindow()
        {
            GetWindow<ML_AddRobot>(windowTitle);
        }
        private void OnGUI()
        {
            GUILayout.Label(introText, EditorStyles.boldLabel);
            GUILayout.Label(introText_con, EditorStyles.boldLabel);
            GUILayout.Space(2);
            GUILayout.Label(introText_last, EditorStyles.boldLabel);
            GUILayout.Space(10);

            //Robot Setup
            GUILayout.Label("Robot Setup", EditorStyles.boldLabel);
            robotName = EditorGUILayout.TextField(new GUIContent("Robot Name", "Defines the name of the robot object. (Default = GenericRobot)"), robotName);

            EditorGUI.BeginChangeCheck();
            robotPrefab = EditorGUILayout.ObjectField(new GUIContent("Robot Prefab", "Define the Robot Prefab to be added. Samples are availble under 'Assets>MirrorLabs>Prefabs>Robots'."), robotPrefab, typeof(GameObject), true) as GameObject;

            //Prefab Preview within Editor Window
            GUIStyle bgColor = new GUIStyle();

            if (robotPrefab != null)
            {
                if (robotPrefabPreview == null)
                    robotPrefabPreview = Editor.CreateEditor(robotPrefab);
                bgColor.normal.background = EditorGUIUtility.whiteTexture;

                robotPrefabPreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Robot Prefab changed");
                try
                {
                    if (robotPrefab != null)
                    {
                        Editor.DestroyImmediate(robotPrefabPreview);
                        robotPrefabPreview = Editor.CreateEditor(robotPrefab);
                        bgColor.normal.background = EditorGUIUtility.whiteTexture;
                        robotPrefabPreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            GUILayout.Space(10);

            //ROS# Configuration
            GUILayout.Label("ROS Setup", EditorStyles.boldLabel);

            includeROSSharp = EditorGUILayout.BeginToggleGroup(new GUIContent("Inlcude ROS", "Add and configure the ROS communication based on parameters below."), includeROSSharp);
            GUILayout.Label("Please define the desired ROS configuration.");
            GUILayout.Label("Subscribers", EditorStyles.label);
            isJointSubscribed = EditorGUILayout.Toggle(new GUIContent("     Joint States", "Subscribe to the '/joint_states' topic for your robot."), isJointSubscribed);
            isOdomSubscribed = EditorGUILayout.Toggle(new GUIContent("     Odometry", "Subscribe to the '/odom' topic for your robot."), isOdomSubscribed);

            GUILayout.Space(5);

            GUILayout.Label("Publishers", EditorStyles.label);
            isJointPublisher = EditorGUILayout.Toggle(new GUIContent("     Joint States", "Subscribe to the '/joint_states' topic for your robot."), isJointPublisher);
            isPoseStamp = EditorGUILayout.Toggle(new GUIContent("     Pose Stamped", "Publish Pose Stamps from Unity under '/pose_stamped'."), isPoseStamp);

            EditorGUILayout.EndToggleGroup();


            //Initiate the generating of the new scene according to given parameters.
            if (GUILayout.Button(new GUIContent("Add Robot", "Click to confirm the configuration of the new robot.")))
            {
                Debug.Log("Adding Robot");
                AddRobot(robotName, robotPrefab);
                if (includeROSSharp)
                {
                    ConfigureROSConnector(robotName, isJointSubscribed, isOdomSubscribed, isJointPublisher, isPoseStamp);
                }

            }
        }

        private void AddRobot(string robotName, GameObject robotObject)
        {
            //Instatiate Robot Parent Object
            GameObject robotParentObject;
            if ( GameObject.Find("Robots")== null)
            {
                robotParentObject = new GameObject("Robots");
                robotParentObject.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                robotParentObject = GameObject.Find("Robots");
            }

            //Add Robot to Robot-parent 
            robotName = (robotName == "" || robotName == null) ? "Generic Robot" : robotName;
           
            robotObj = PrefabUtility.InstantiatePrefab(robotObject) as GameObject;
            robotObj.transform.parent = robotParentObject.transform;



            //changing the name of the robot. 
            /* Note: For some reason we have to do it via the parent transform. Making changes on the object directly results in 'interesting' behaviours.
             */

            robotParentObject.transform.GetChild(robotParentObject.transform.childCount - 1).gameObject.name = robotName;
            robotObj = robotParentObject.transform.GetChild(robotParentObject.transform.childCount - 1).gameObject;


            //Instiate ROSConnector Parent Object
            
            if (GameObject.Find("RosConnectors") == null)
            {
                rosParentObject = new GameObject("RosConnectors");
                rosParentObject.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                rosParentObject = GameObject.Find("RosConnectors");
            }
            

            //Add RosConnector
            rosConnectorObj = PrefabUtility.LoadPrefabContents("Assets/MirrorLab/Prefabs/ROSConnector.prefab") as GameObject;
            
            rosConnectorObj.transform.parent = rosParentObject.transform;
            rosParentObject.transform.GetChild(rosParentObject.transform.childCount - 1).gameObject.name = robotName;


            rosConnectorObj = rosParentObject.transform.GetChild(rosParentObject.transform.childCount - 1).gameObject;
            if (rosConnectorObj.GetComponent<JointStatePatcher>() != null)
            {
                rosConnectorObj.GetComponent<JointStatePatcher>().UrdfRobot = robotObj.GetComponent<UrdfRobot>();
            }
            else
            {
                Debug.Log("No URDF Component found on added robot. Please atatch manually");
            }
        }
        private void ConfigureROSConnector(string robotName, bool jointSubscribed, bool odomSubscribed, bool jointPublish, bool poseStamped)
        {
            //GameObject robotObj = GameObject.Find("Ros);
            var js = rosConnectorObj.GetComponent<JointStateSubscriber>();
            var jp = rosConnectorObj.GetComponent<JointStatePublisher>();
            var os = rosConnectorObj.GetComponent<OdometrySubscriber>();
            var psp = rosConnectorObj.GetComponent<CustomPoseStampedPublisher>();
            if (robotName == "" || robotName == null)
            {
                robotName = "GenericRobot";
            }

            js.Topic = "/" + robotName + "/" + "joint_states";
            jp.Topic = "/" + robotName + "/" + "joint_states";
            os.Topic = "/" + robotName + "/" + "odom";
            psp.Topic = "/" + robotName + "/" + "pose_stamped";

            js.enabled = jointSubscribed;
            jp.enabled = jointPublish;
            os.enabled = odomSubscribed;
            psp.enabled = poseStamped;
        }
    }
}