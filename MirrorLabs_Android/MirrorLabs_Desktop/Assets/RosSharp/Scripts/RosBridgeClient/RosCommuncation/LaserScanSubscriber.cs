﻿/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

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

//using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using Unity;
using System;

namespace RosSharp.RosBridgeClient
{
    public class LaserScanSubscriber : Subscriber<Messages.Sensor.LaserScan>
    {
        public LaserScanWriter laserScanWriter;

        protected override void Start()
        {
            base.Start();
            Debug.Log("LaserScanSubscriber started");
        }

        protected override void ReceiveMessage(Messages.Sensor.LaserScan laserScan)
        {
            try
            {
                Debug.Log("Received Laser Message");
                laserScanWriter.Write(laserScan);
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}