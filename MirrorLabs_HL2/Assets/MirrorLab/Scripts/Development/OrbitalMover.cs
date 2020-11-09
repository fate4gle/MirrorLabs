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
    public class OrbitalMover : MonoBehaviour
    {
        public float orbitDiameter;
        public Vector3 orbitOffset;
        public float speed;

        void Update()
        {
            Vector3 orbit = new Vector3(Mathf.Sin(Time.time * speed) * orbitDiameter
                                        , Mathf.Sin(Time.time * speed / 3) * orbitDiameter / 1.5f
                                        , Mathf.Cos(Time.time * speed)) * orbitDiameter
                + orbitOffset;

            this.transform.localPosition = orbit;
            transform.LookAt(new Vector3(0, 0, 0));
        }
    }
}
