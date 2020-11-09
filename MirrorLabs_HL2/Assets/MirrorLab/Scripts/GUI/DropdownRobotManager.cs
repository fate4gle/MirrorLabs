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

using Microsoft.MixedReality.Toolkit.UI;


using UnityEngine;
using UnityEngine.UI;



namespace MirrorLabs
{
    public class DropdownRobotManager : MonoBehaviour
    {

        public GameObject[] rosConnectors;

        [SerializeField]
        private Settings_RosModeManager settings_RosModeManager;
        [SerializeField]
        private Settings_IpConfigManager settings_IpConfigManager;
        [SerializeField]
        private RobotControl_PoseSelectionManager robotControl_PoseSelectionManager;

        public RobotConfig[] robotConfigs;
        private int num_RosConnectors;

        public int currentRobot = 0;

        public ButtonConfigHelper currentRobotLabel;


        private void Awake()
        {

            num_RosConnectors = GameObject.Find("RosConnectors").transform.childCount;
            rosConnectors = new GameObject[num_RosConnectors];
            robotConfigs = new RobotConfig[num_RosConnectors];
        }
        // Start is called before the first frame update
        void Start()
        {
            initConfig();

            currentRobotLabel.MainLabelText = robotConfigs[0].robotName;
            Debug.Log(robotConfigs[0].robotName);

        }



        
        
        
        
        void initConfig()
        {
            robotConfigs = new RobotConfig[num_RosConnectors];
            for (int i = 0; i < num_RosConnectors; i++)
            {
                // initiate the robotConfigs for all available rosconnectors
                robotConfigs[i] = new RobotConfig();
                Debug.Log("new RobotConfig generated");
                robotConfigs[i].rosMode = 0;
                robotConfigs[i].poseMode = 0;
                robotConfigs[i].isManualPose = false;
                robotConfigs[i].isIterativeMode = false;
                robotConfigs[i].isPoseMode = false;
                robotConfigs[i].poseInterval = 10f;

                //robotConfigs[i].robotName = GameObject.Find("RosConnectors").transform.GetChild(i).name;
                robotConfigs[i].robotName = GameObject.Find("RosConnectors").transform.GetChild(i).name;
                Debug.Log(GameObject.Find("RosConnectors").transform.GetChild(i).name);

                Debug.Log(robotConfigs[i].robotName);
            }


            // get the reference for each rosconnector
            for (int i = 0; i < num_RosConnectors; i++)
            {
                rosConnectors[i] = GameObject.Find("RosConnectors").transform.GetChild(i).gameObject;
            }
            RobotChanged(0);

        }

        void RobotDropdownValueChanged(Dropdown dp)
        {

            settings_IpConfigManager.NewRobotSelected(rosConnectors[dp.value]);
            settings_RosModeManager.NewRobotSelected(rosConnectors[dp.value]);
            robotControl_PoseSelectionManager.NewRobotSelected(rosConnectors[dp.value]);
        }
        void RobotChanged(int i)
        {

            currentRobotLabel.MainLabelText = robotConfigs[i].robotName;
            settings_IpConfigManager.NewRobotSelected(rosConnectors[i]);
            settings_RosModeManager.NewRobotSelected(rosConnectors[i]);
            robotControl_PoseSelectionManager.NewRobotSelected(rosConnectors[i]);

        }

        public void NextRobot()
        {

            if (currentRobot == num_RosConnectors - 1)
            {
                currentRobot = 0;
            }
            else
            {
                currentRobot++;
            }
            RobotChanged(currentRobot);

        }
        public void PreviousRobot()
        {

            if (currentRobot == 0)
            {
                currentRobot = num_RosConnectors - 1;
            }
            else
            {
                currentRobot--;
            }
            RobotChanged(currentRobot);

        }

    }
}

