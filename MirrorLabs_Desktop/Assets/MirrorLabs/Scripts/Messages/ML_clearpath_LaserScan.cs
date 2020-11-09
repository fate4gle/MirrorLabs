#region Assembly RosBridgeClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\jonas\Documents\Git\TUD\Mirror_Labs\MirrorLabs_Desktop\Assets\RosSharp\Plugins\RosBridgeClient.dll
#endregion

using Newtonsoft.Json;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Standard;

namespace RosSharp.RosBridgeClient.Messages.Sensor
{
    public class ML_clearpath_LaserScan : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "sensor_msgs/LaserScan";
        public Header header;
        public float angle_min;
        public float angle_max;
        public float angle_increment;
        public float time_increment;
        public float scan_time;
        public float range_min;
        public float range_max;
        public float[] ranges;
        public float[] intensities;

        public ML_clearpath_LaserScan() 
        {
            angle_min = new float();
            angle_max = new float();
            angle_increment = new float();
            time_increment = new float();
            scan_time = new float();
            range_min = new float();
            range_max = new float();
            ranges = new float[0];
            intensities = new float[0];
        }
    }
}