public interface IDynamicLightAdjuster
{
    double GetAppropriateBrightness(double illumination, double currentBrightness);

    /// <summary>
    /// Model required for logic
    /// </summary>
    public class DynamicLightModel
    {
        /// <summary>
        /// The target which the algoritm should try to hit
        /// </summary>
        public int TargetIllumination { get; set; }
        /// <summary>
        /// When turning on the light, what is the minimum brighness needed to activate the lights
        /// </summary>
        public int MinBrightness { get; set; }
        /// <summary>
        /// The maximum brightness that lights should be set to
        /// </summary>
        public int MaxLightBrightness { get; set; }
        /// <summary>
        /// The amount of illumination added when lights are set to minimum 
        /// </summary>
        public int IlluminationAddedAtMin { get; set; }
        /// <summary>
        /// The amount of illumination added when lights are set to maximum 
        /// </summary>
        public int IlluminationAddedAtMax { get; set; }    

        /// <summary>
        /// The minimum value usually reported by the sensor
        /// Needs consideration
        /// </summary>
        //public int MinIllumination { get; set; } = 0;
    }
} 