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

using UnityEngine;
using UnityEngine.UI;


using RosSharp.RosBridgeClient;
using Microsoft.MixedReality.Toolkit.UI;            //Deactivate for non-MRTK applications

namespace MirrorLabs{
    public class Settings_RosModeManager : MonoBehaviour
    {


        [SerializeField]
        private DropdownRobotManager dropdownRobotManager;

        public GameObject robotManager;
        private RosConnector rosConnector;
        private JointStatePublisher jointStatePublisher;
        private JointStateSubscriber jointStateSubscriber;
        private CustomPoseStampedPublisher poseStampedPublisher;
        




        [SerializeField]
        private Dropdown rosModeDropdown;
        [SerializeField]
        private Dropdown poseStampDropdown;
        [SerializeField]
        //private Slider intervalSlider;                    //Use standard slider for non-MRTK applications
        private PinchSlider intervalSlider;

        private int rosMode;
        private int poseMode;
        private bool isPoseMode = false;
        private bool isIterativeMode = false;
        private bool isManualPose = false;
        private bool isAutomaticPose = false;
        private float iterativeInterval = 10f;




        void Start()
        {
            rosModeDropdown.onValueChanged.AddListener(delegate
            {
                RosModeDropdownValueChanged(rosModeDropdown);
            });
            poseStampDropdown.onValueChanged.AddListener(delegate
            {
                PoseStampDropdownValueChanged(poseStampDropdown);
            });

        }


        public void NewRobotSelected(GameObject robot)
        {
            

            if (robot.GetComponent<RosConnector>() == null)
            {
                Debug.Log("Error: No rosConnector found");
                return;
            }

            robotManager = robot;
            rosConnector = robotManager.GetComponent<RosConnector>();
            jointStatePublisher = robotManager.GetComponent<JointStatePublisher>();
            jointStateSubscriber = robotManager.GetComponent<JointStateSubscriber>();
            poseStampedPublisher = robotManager.GetComponent<CustomPoseStampedPublisher>();
            rosMode = dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].rosMode;
            poseMode = dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].poseMode;
            rosModeDropdown.value = rosMode;
            poseStampDropdown.value = poseMode;


            //intervalSlider.value = dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].poseInterval;   //Activate for non-MRTK applications
            intervalSlider.SliderValue = dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].poseInterval;
        }

        void RosModeDropdownValueChanged(Dropdown dp)
        {
            if (dp.value != 0)
            {
                poseStampDropdown.gameObject.SetActive(true);
            }
            else
            {
                poseStampDropdown.gameObject.SetActive(false);
            }
        }
        void PoseStampDropdownValueChanged(Dropdown dp)
        {
            if (dp.value == 1)
            {
                intervalSlider.gameObject.SetActive(true);
            }
            else
            {
                intervalSlider.gameObject.SetActive(false);
            }
        }



        public void ConfirmRosModeSettings()
        {
            /*  RosMode options
             *  0 -> Reactive Only 
             *  1 -> PoseStamps based (interactive)
             *  
             */

            if (rosModeDropdown.value != 0)
            {
                // interactive
                jointStatePublisher.enabled = false;
                jointStateSubscriber.enabled = true;


                if (poseStampDropdown.value == 0)
                {
                    //Manual Mode
                    isPoseMode = true;
                    isIterativeMode = false;
                    isManualPose = true;





                }
                else if (poseStampDropdown.value == 1)
                {
                    // Automatic Iterative
                    isPoseMode = true;
                    isIterativeMode = true;
                    isManualPose = false;





                }
                else if (poseStampDropdown.value == 2)
                {
                    // Automatic OnRelease
                    isPoseMode = true;
                    isIterativeMode = false;
                    isManualPose = false;





                }
            }
            else
            {
                //Reactive Only. Commands are received from Ros only, one-way communication.
                isPoseMode = false;
                isIterativeMode = false;
                isManualPose = false;
            }
            //iterativeInterval = intervalSlider.value;                     //Activate for non-MRTK applications
            iterativeInterval = intervalSlider.SliderValue;
            jointStatePublisher.enabled = false;
            jointStateSubscriber.enabled = true;
            poseStampedPublisher.enabled = isPoseMode;
            poseStampedPublisher.InitPublishing(isManualPose, isAutomaticPose, isIterativeMode, iterativeInterval);

            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].poseMode = poseStampDropdown.value;
            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].rosMode = rosModeDropdown.value;
            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = isPoseMode;
            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = isIterativeMode;
            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = isManualPose;
            dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].poseInterval = iterativeInterval;


        }
    }
}
