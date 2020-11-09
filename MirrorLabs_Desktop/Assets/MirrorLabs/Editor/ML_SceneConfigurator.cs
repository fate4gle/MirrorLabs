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
using UnityEditor.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

using RosSharp;
using RosSharp.Urdf;
using RosSharp.RosBridgeClient;







//using Microsoft.MixedReality.Toolkit;
namespace MirrorLabs
{
    public class MLSceneConfigurator : EditorWindow
    {


        private static string windowTitle = "MirrorLabs Scene Configurator";
        private string introText = "Please configure a new scene and click on the Button to generate it!";
        
        //Scene Setup
        private string sceneName = "New MirrorLabs Scene";
        private string scenePath = "Assets/MirrorLabs/Scenes/";
        private string prefabPath = "Assets/MirrorLabs/Prefabs/";
        private bool includeLighting = true;

        //Robot Setup
        public GameObject robotPrefab;
        public string robotName;
        Editor robotPrefabPreview;
        public GameObject robot;
        public GameObject robotObj;

        //ROS# Setup
        private bool includeROSSharp = true;
        private bool isJointSubscribed = false;
        private bool isOdomSubscribed = false;
        private bool isJointPublisher = false;
        private bool isPoseStamp = false;
        public GameObject rosConnectorObj;

        //MRTK Setup
        private bool includeMRTK = false;
        private bool isHoloLens1 = false;
        private bool isHoloLens2 = false;

        







        [MenuItem("MirrorLabs/SceneConfigurator")]
        public static void ShowWindow()
        {
            GetWindow<MLSceneConfigurator>(windowTitle);
        }





        private void OnGUI()
        {
            GUILayout.Label(introText, EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Space(10);

            //Scene Setup
            GUILayout.Label("Scene", EditorStyles.boldLabel);
            sceneName = EditorGUILayout.TextField("Scene Name", sceneName);
            includeLighting = EditorGUILayout.Toggle(new GUIContent("Automatic Lighting", "Add 'Directional Lights' to the scene."), includeLighting);
            GUILayout.Space(10);


            //Robot Setup
            GUILayout.Label("Robot Setup", EditorStyles.boldLabel);
            robotName = EditorGUILayout.TextField(new GUIContent("Robot Name", "Defines the name of the robot object. (Default = GenericRobot)"), robotName);
            
            EditorGUI.BeginChangeCheck();
            robotPrefab = EditorGUILayout.ObjectField(new GUIContent("Robot Prefab", "Define the Robot Prefab to be added. Samples are availble under 'Assets>MirrorLabs>Prefabs>Robots'."), robotPrefab, typeof (GameObject), true) as GameObject;

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
                catch(Exception e)
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
                isOdomSubscribed =  EditorGUILayout.Toggle(new GUIContent("     Odometry", "Subscribe to the '/odom' topic for your robot."), isOdomSubscribed);

                GUILayout.Space(5);

                GUILayout.Label("Publishers", EditorStyles.label);
                isJointPublisher = EditorGUILayout.Toggle(new GUIContent("     Joint States", "Subscribe to the '/joint_states' topic for your robot."), isJointPublisher);
                isPoseStamp =  EditorGUILayout.Toggle(new GUIContent("     Pose Stamped", "Publish Pose Stamps from Unity under '/pose_stamped'."), isPoseStamp);

            EditorGUILayout.EndToggleGroup();

            GUILayout.Space(5);


            


            //Initiate the generating of the new scene according to given parameters.
            if (GUILayout.Button(new GUIContent("Generate Scene", "Press to start the scene generator. This may take a moment.")))
            {
                Debug.Log("Generating new Scene");
                GenerateScene(sceneName, robotPrefab, robotName);
            }
        }


        void GenerateScene(string name, GameObject robot, string robotName)
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            newScene.name = name;

            GameObject cam = new GameObject("MainCamera");
            cam.AddComponent<Camera>();
            cam.AddComponent<AudioListener>();
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0, 1.25f, -1.5f);
            cam.transform.rotation = Quaternion.Euler(new Vector3(20, 0, 0));

            
            if (includeLighting)
            {
                AddLighting();
            }
            if (robotPrefab != null)
            {
                Debug.Log("Adding Robot to Scene...");
                AddRobot(robotName, robot);
            }
            
            if (includeROSSharp)
            {
                ConfigureROSConnector(robotName, isJointSubscribed, isOdomSubscribed, isJointPublisher, isPoseStamp);
            }

            EditorSceneManager.SaveScene(newScene, scenePath + name +".unity");
            this.Close();
        }

        private void AddLighting()
        {
            //Instantiate new Lighting Parent Object
            GameObject Lighting = new GameObject("Lighting");
            Lighting.transform.position = new Vector3(0, 2, 0);

            //Instantiate the actual generic directional Light and configure it
            GameObject dl = new GameObject("Directional Light");
            dl.transform.rotation = Quaternion.Euler(45, 45, 0);

            var light = dl.gameObject.AddComponent<Light>();
            light.type = UnityEngine.LightType.Directional;
            dl.transform.parent = Lighting.transform;
            dl.transform.localPosition = new Vector3(0, 0, 0);
        }

        private void AddRobot(string robotName, GameObject robotObject)
        {
            //Instatiate Robot Parent Object
            GameObject robotParentObject = new GameObject("Robots");
            robotParentObject.transform.position = new Vector3(0, 0, 0);

            //Add Robot to Robot-parent 
            robotName = (robotName == ""|| robotName == null) ? "Generic Robot" : robotName;            
            robotObj = PrefabUtility.InstantiatePrefab(robotObject) as GameObject;
            robotObj.transform.parent = robotParentObject.transform;



            //changing the name of the robot. 
            /* Note: For some reason we have to do it via the parent transform. Making changes on the object directly results in 'interesting' behaviours.
             */
            
            robotParentObject.transform.GetChild(robotParentObject.transform.childCount-1).gameObject.name = robotName;
            robotObj = robotParentObject.transform.GetChild(robotParentObject.transform.childCount - 1).gameObject;


            //Instiate ROSConnector Parent Object
            GameObject rosParentObject = new GameObject("RosConnectors");
            rosParentObject.transform.position = new Vector3(0, 0, 0);

            //Add RosConnector
            rosConnectorObj = PrefabUtility.LoadPrefabContents("Assets/MirrorLabs/Prefabs/ROSConnector.prefab") as GameObject;
            Debug.Log(rosConnectorObj);
            rosConnectorObj.transform.parent = rosParentObject.transform;
            rosParentObject.transform.GetChild(rosParentObject.transform.childCount - 1).gameObject.name = robotName;


            rosConnectorObj = rosParentObject.transform.GetChild(rosParentObject.transform.childCount - 1).gameObject;
            if(rosConnectorObj.GetComponent<JointStatePatcher>() != null)
            {
                rosConnectorObj.GetComponent<JointStatePatcher>().UrdfRobot = robotObj.GetComponent<UrdfRobot>();
            }
            else
            {
                Debug.Log("No URDF Component found on added robot. Please atatch manually");
            }

            // Add Unity Main Thread Dispatcher
            GameObject umtd = new GameObject("MainThreadDispatcher");
            umtd.AddComponent<UnityMainThreadDispatcher>();
            umtd.transform.position = new Vector3(0, 0, 0);

        }

        
        private void ConfigureROSConnector(string robotName, bool jointSubscribed, bool odomSubscribed, bool jointPublish, bool poseStamped)
        {
            
            var jpatch = rosConnectorObj.GetComponent<JointStatePatcher>();
            var js = rosConnectorObj.GetComponent<JointStateSubscriber>();
            var jp = rosConnectorObj.GetComponent<JointStatePublisher>();
            var os = rosConnectorObj.GetComponent<OdometrySubscriber>();
            var psp = rosConnectorObj.GetComponent<CustomPoseStampedPublisher>();
            if(robotName == "" || robotName == null)
            {
                robotName = "GenericRobot";
            }

            jpatch.SetSubscribeJointStates(jointSubscribed);
            jpatch.SetPublishJointStates(jointPublish);
            js.Topic = "/" + robotName + "/" + "joint_states";
            jp.Topic = "/" + robotName + "/" + "joint_states";
            os.Topic = "/" + robotName + "/" + "odom";
            psp.Topic = "/" + robotName + "/" + "pose_stamped";

            js.enabled = jointSubscribed;
            jp.enabled = jointPublish;
            os.enabled = odomSubscribed;
            psp.enabled = poseStamped;
                        
        }

        
        public void OnDisable()
        {
            Debug.Log("Closing ML Scene Configurator");
        }
    }
}


