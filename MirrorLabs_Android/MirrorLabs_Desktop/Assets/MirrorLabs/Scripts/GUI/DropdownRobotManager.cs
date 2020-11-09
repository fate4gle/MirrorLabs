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



namespace MirrorLabs
{
    public class DropdownRobotManager : MonoBehaviour
    {

        public GameObject[] rosConnectors;
        public Dropdown robotDropdown;
        [SerializeField]
        private Settings_RosModeManager settings_RosModeManager;
        [SerializeField]
        private Settings_IpConfigManager settings_IpConfigManager;
        [SerializeField]
        private RobotControl_PoseSelectionManager robotControl_PoseSelectionManager;

        public RobotConfig[] robotConfigs;
        private int num_RosConnectors;


        private void Awake()
        {
            robotDropdown = this.GetComponent<Dropdown>();
            num_RosConnectors = GameObject.Find("RosConnectors").transform.childCount;
            rosConnectors = new GameObject[num_RosConnectors];
            robotConfigs = new RobotConfig[num_RosConnectors];
        }
        // Start is called before the first frame update
        void Start()
        {
            initDropdown();
            robotDropdown.onValueChanged.AddListener(delegate
            {
                RobotDropdownValueChanged(robotDropdown);
            });
        }



        void initDropdown()
        {
            for (int i = 0; i < num_RosConnectors; i++)
            {
                robotConfigs[i] = new RobotConfig();
                robotConfigs[i].rosMode = 0;
                robotConfigs[i].poseMode = 0;
                robotConfigs[i].isManualPose = false;
                robotConfigs[i].isIterativeMode = false;
                robotConfigs[i].isPoseMode = false;
                robotConfigs[i].poseInterval = 10f;
            }
            
            for (int i = 0; i < num_RosConnectors; i++)
            {
                rosConnectors[i] = GameObject.Find("RosConnectors").transform.GetChild(i).gameObject;
            }
            robotDropdown.ClearOptions();
            foreach (GameObject g in rosConnectors)
            {
                robotDropdown.options.Add(new Dropdown.OptionData() { text = g.name });
            }
            robotDropdown.value = 0;
            RobotDropdownValueChanged(robotDropdown);
        }

        void RobotDropdownValueChanged(Dropdown dp)
        {
            
            settings_IpConfigManager.NewRobotSelected(rosConnectors[dp.value]);
            settings_RosModeManager.NewRobotSelected(rosConnectors[dp.value]);
            robotControl_PoseSelectionManager.NewRobotSelected(rosConnectors[dp.value]);
        }
    }
}
