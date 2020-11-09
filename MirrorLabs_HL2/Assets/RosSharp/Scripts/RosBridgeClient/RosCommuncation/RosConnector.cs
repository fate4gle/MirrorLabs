/*
© Siemens AG, 2017
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

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
using System.Collections;
using System.Threading;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class RosConnector : MonoBehaviour
    {
        public int Timeout = 10;

        public RosSocket RosSocket { get; private set; }
        public enum Protocols { WebSocketSharp, WebSocketNET, WebSocketUWP };
        public RosBridgeClient.RosSocket.SerializerEnum Serializer;
        public Protocols Protocol;
        public string RosBridgeServerUrl = "ws://192.168.0.1:9090";

        private ManualResetEvent isConnected = new ManualResetEvent(false);


        /* ----------------------------------------------------------------------------------------------------------------------
         * Addition made by J.S.I. Rieder (TU Delft)
         * Goal: Change the remote IP/Port address dynamically.
         * 
         * Realization: User has a Screenspace overlay (Graphical User Interface [GUI]) where the IP/Port can directly be typed in 
         * and the ros-bridge can be activated using a button.
         * 
         * Changes made in this script:
         * 1) Implementation of public void for user-intended initialization. (The GUI button triggers this void)
         * 1.1) Closes the existing socket 
         * 1.2) Reinitializes the connection once the rosbridgeServerUrl is updated. (Point 2)
         * 2) the rosBridgeServerUrl can be overwritten using the remoteIP input field on the GUI
         * 
         */

        private string RosBridgeServer_Prefix = "ws://";
        public string RosBridgeServer_IP = "192.168.0.101";
        public string RosBridgeServer_Port = "9090";



        public void InitRosConnector(string IP, string Port)
        {
            //Close existing RosSocket
            try
            {
                RosSocket.Close();
                this.GetComponent<JointStateSubscriber>().enabled = false;

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            RosBridgeServer_Port = Port;
            RosBridgeServer_IP = IP;



            //Overwrite rosBridgeServerUrl
            RosBridgeServerUrl = RosBridgeServer_Prefix + RosBridgeServer_IP + ":" + RosBridgeServer_Port;

            new Thread(ConnectAndWait).Start();

        }
        public void GUI_SetPort(string s)
        {
            RosBridgeServer_Port = s;
        }
        public void GUI_SetIP(string s)
        {
            RosBridgeServer_IP = s;
        }

        public void DispatchToMainThread_RosConnectionSuccess()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(DispactchedRosConnectionSuccess());
        }
        public IEnumerator DispactchedRosConnectionSuccess()
        {
            yield return new WaitForSeconds(1);
            RosConnectionSuccess();
            yield return null;
        }
        private void RosConnectionSuccess()
        {
            this.gameObject.GetComponent<JointStateSubscriber>().enabled = true;
        }


        // --------------------------------------------------------------------------------------------------

        public void Awake()
        {
#if WINDOWS_UWP
            ConnectAndWait();
#else
            new Thread(ConnectAndWait).Start();
#endif
        }

        private void ConnectAndWait()
        {
            RosSocket = ConnectToRos(Protocol, RosBridgeServerUrl, OnConnected, OnClosed,Serializer);

            if (!isConnected.WaitOne(Timeout * 1000))
                Debug.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);
        }
        
        public static RosSocket ConnectToRos(Protocols protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null,RosSocket.SerializerEnum serializer=RosSocket.SerializerEnum.JSON)
        {
            RosBridgeClient.Protocols.IProtocol protocol = GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol,serializer);
        }

        private static RosBridgeClient.Protocols.IProtocol GetProtocol(Protocols protocol, string rosBridgeServerUrl)
        {

#if WINDOWS_UWP
                return new RosBridgeClient.Protocols.WebSocketUWPProtocol(rosBridgeServerUrl);
#else
            switch (protocol)
            {
                case Protocols.WebSocketNET:
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(rosBridgeServerUrl);
                case Protocols.WebSocketSharp:
                    return new RosBridgeClient.Protocols.WebSocketSharpProtocol(rosBridgeServerUrl);
                case Protocols.WebSocketUWP:
                    Debug.Log("WebSocketUWP only works when deployed to HoloLens, defaulting to WebSocketNetProtocol");
                    return new RosBridgeClient.Protocols.WebSocketNetProtocol(rosBridgeServerUrl);
                default:
                    return null;
            }
#endif
        }

        private void OnApplicationQuit()
        {
            RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            isConnected.Set();
            Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);

            // Dispatch event to main thread
            try
            {
                DispatchToMainThread_RosConnectionSuccess();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            isConnected.Reset();
            Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
        }
    }
}
