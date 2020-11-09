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

namespace MirrorLabs
{
    public class RobotConfig: Object
    {

        //This class is only stores the configurations made via the GUI 
        public int rosMode { get; set; } 
        public int poseMode { get; set; }
        public float poseInterval { get; set; }

        public bool isPoseMode { get; set; }
        public bool isIterativeMode { get; set; }
        public bool isManualPose { get; set; }
        public string robotName { get; set; }
        
    }
}
