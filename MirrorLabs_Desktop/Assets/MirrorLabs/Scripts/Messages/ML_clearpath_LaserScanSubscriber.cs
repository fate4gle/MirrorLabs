//using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using Unity;

namespace RosSharp.RosBridgeClient
{
    public class ML_clearpath_LaserScanSubscriber : Subscriber<Messages.Sensor.ML_clearpath_LaserScan>
    {
        public ML_clearpath_LaserScanWriter laserScanWriter;

        protected override void Start()
        {
            base.Start();
            Debug.Log("LaserScanSubscriber started");
        }

        protected override void ReceiveMessage(Messages.Sensor.ML_clearpath_LaserScan laserScan)
        {
            Debug.Log("Received Laser Message");
            laserScanWriter.Write(laserScan);

        }
    }
}