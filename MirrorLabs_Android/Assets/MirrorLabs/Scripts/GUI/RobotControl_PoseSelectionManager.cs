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

namespace MirrorLabs
{
    public class RobotControl_PoseSelectionManager : MonoBehaviour
    {
        // Protocol needs to be established in ROS


        [SerializeField]
        private DropdownRobotManager dropdownRobotManager;
        private GameObject robotObj;
        private RosConnector rosConnector;



        [SerializeField]
        private Button[] PoseBtns;
        [SerializeField]
        private int n_Btn;


        void Start()
        {
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



        }
        public void NewRobotSelected(GameObject robotObject)
        {
            //Debug.Log("new Robot Selected: " + robotObject.name);

            if (robotObject.GetComponent<RosConnector>() == null)
            {
                Debug.Log("Error: No rosConnector found");
                return;
            }

            robotObj = robotObject;
            rosConnector = robotObj.GetComponent<RosConnector>();

        }

        void SetHomePose()
        {
            Debug.Log("HomePose Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose

        }
        void SetPose1()
        {
            Debug.Log("Pose 1 Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose

        }
        void SetPose2()
        {
            Debug.Log("Pose 2 Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose
        }
        void SetPose3()
        {
            Debug.Log("Pose 3 Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose
        }
        void SetPose4()
        {
            Debug.Log("Pose 4 Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose
        }
        void SetPose5()
        {
            Debug.Log("Pose 5 Selected");
            if (!dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose)
            {
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isManualPose = true;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isIterativeMode = false;
                dropdownRobotManager.robotConfigs[dropdownRobotManager.robotDropdown.value].isPoseMode = true;
            }

            //Send Pose
        }

    }
}
