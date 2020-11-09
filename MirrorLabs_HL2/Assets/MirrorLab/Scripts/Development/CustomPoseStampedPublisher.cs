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

namespace RosSharp.RosBridgeClient
{
    public class CustomPoseStampedPublisher : Publisher<Messages.Geometry.PoseStamped>
    {
        public Transform PublishedTransform;
        public string FrameId = "Unity";

        private Messages.Geometry.PoseStamped message;
        private bool isAutomatic = false;
        private bool isIterative = false;
        private bool isManual = false;
        private float publishInterval = 10f;
        private float publishIntervalTimer = 0;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate() 
        {
            if (isAutomatic)
            {
                if (isIterative)
                {
                    publishIntervalTimer += Time.deltaTime;
                    if (publishIntervalTimer >= publishInterval)
                    {
                        publishIntervalTimer = 0;
                        UpdateMessage();

                    }
                }
                else
                {
                    UpdateMessage();
                }
            }
            
        }

        private void InitializeMessage()
        {
            message = new Messages.Geometry.PoseStamped
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };
        }

        private void UpdateMessage()
        {
            message.header.Update();
            message.pose.position = GetGeometryPoint(PublishedTransform.position.Unity2Ros());
            message.pose.orientation = GetGeometryQuaternion(PublishedTransform.rotation.Unity2Ros());

            Publish(message);
        }

        private Messages.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            Messages.Geometry.Point geometryPoint = new Messages.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }
        public void InitPublishing(bool manual,bool automatic, bool iterative, float iterativeInterval)
        {
            isManual = manual;
            isAutomatic = automatic;
            isIterative = iterative;
            publishInterval = iterativeInterval;
        }

    }
}
