namespace MyHome;

/// <summary>
/// Provides a means of adjusting lights when used with a light sensor to maintain a consistent light level given variability of an outside light source (the sun).
/// </summary>
public class DynamicLightAdjuster : IDynamicLightAdjuster
{
    IDynamicLightAdjuster.DynamicLightModel _model;
    double _m, _b;


    public DynamicLightAdjuster(IDynamicLightAdjuster.DynamicLightModel model)
    {
        _model = model;

        // slope and y intercpt for use later
        _m = (double)(_model.IlluminationAddedAtMax - _model.IlluminationAddedAtMin)/(double)(_model.MaxLightBrightness - _model.MinBrightness);
        _b = _model.IlluminationAddedAtMax - _m * _model.MaxLightBrightness;
    }

    public double GetAppropriateBrightness(double illumination, double currentBrightness)
    {
        //calulate the illumination that is being added by the light
        var illuminationAddedByLight = GetIlluminationFromBrightness(currentBrightness);
        
        // subtract it from the illumination
        var actualIllumination = illumination - illuminationAddedByLight;

        //calculate what it should be and return
        return GetBrightnessFromIllumination(_model.TargetIllumination - actualIllumination);
    }

    double GetIlluminationFromBrightness(double currentBrightness)
    {
        if (currentBrightness == 0) return 0;

        // y = mx + b
        return _m * currentBrightness + _b;
    }

    double GetBrightnessFromIllumination(double illuminationToAdd)
    {
        // x = (y-b)/m
        var result = (illuminationToAdd - _b)/_m;
        if(result < _model.MinBrightness) return 0;
        if(result > _model.MaxLightBrightness) return _model.MaxLightBrightness;
        return result;
    }
}
