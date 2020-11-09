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


#if UWP || UNITY_EDITOR
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Boundary;
#endif

namespace MirrorLabs
{
    public class RobotControl_PoseSelectionManager : MonoBehaviour
    {
        // Protocol needs to be established in ROS


        [SerializeField]
        private DropdownRobotManager dropdownRobotManager;
        private GameObject robotObj;
        private RosConnector rosConnector;

        //If working on a HoloLens application, manually assign the Button.OnClick events for individual poses
#if !UWP || !UNITY_EDITOR
        [SerializeField]
        private Button[] PoseBtns;
#endif
        [SerializeField]
        private int n_Btn;


        void Start()
        {
#if !UWP || !UNITY_EDITOR
            n_Btn = this.transform.childCount;
            PoseBtns = new Button[n_Btn];
            for (int i = 0; i < n_Btn; i++)
            {
                if (this.transform.GetChild(i).GetComponent<Button>() != null)
                {
                    PoseBtns[i] = this.transform.GetChild(i).GetComponent<Button>();
                }
            }
            PoseBtns[0].onClick.AddListener(SetHomePose);
            PoseBtns[1].onClick.AddListener(SetPose1);
            PoseBtns[2].onClick.AddListener(SetPose2);
            PoseBtns[3].onClick.AddListener(SetPose3);
            PoseBtns[4].onClick.AddListener(SetPose4);
            PoseBtns[5].onClick.AddListener(SetPose5);
#else            

#endif






        }
        public void NewRobotSelected(GameObject robotObject)
        {
            //Debug.Log("new Robot Selected: " + robotObject.name);

            if (robotObject.GetComponent<RosConnector>() == null)
            {
                //Debug.Log("Error: No rosConnector found");
                return;
            }

            robotObj = robotObject;
            rosConnector = robotObj.GetComponent<RosConnector>();

        }

        public void SetHomePose()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose

        }
        public void SetPose1()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose

        }
        public void SetPose2()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose3()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose4()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose5()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose6()
        {           
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose7()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose8()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }
        public void SetPose9()
        {
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.currentRobot].isPoseMode = true;
            }

            //Send Pose
        }

    }
}
