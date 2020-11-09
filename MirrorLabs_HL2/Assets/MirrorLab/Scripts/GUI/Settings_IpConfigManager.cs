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
using TMPro;

using RosSharp.RosBridgeClient;

namespace MirrorLabs{
    public class Settings_IpConfigManager : MonoBehaviour
    {

        [SerializeField]
        private DropdownRobotManager dropdownRobotManager;


        private string ipText;
        private string portText;
        private GameObject robotObj;
        private RosConnector rosConnector;

        [SerializeField]
        private TMP_InputField ipField;
        [SerializeField]
        private TMP_InputField portField;

        private void Start()
        {
            //dropdownRobotManager = GameObject.Find("GUI_Desktop/RobotSelectDropdown").GetComponent<DropdownRobotManager>(); // Activate for non-MRTK applications
            dropdownRobotManager = GameObject.Find("GUI_HL2/RobotSelectDropdown").GetComponent<DropdownRobotManager>();

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
            ipText = rosConnector.RosBridgeServer_IP;
            portText = rosConnector.RosBridgeServer_Port;

            ipField.text = ipText;
            portField.text = portText;

        }

        public void ConnectToRobot()
        {
            ipText = ipField.text;
            portText = portField.text;

            rosConnector.InitRosConnector(ipText, portText);
        }
    }
}
