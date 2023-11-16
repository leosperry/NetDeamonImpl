using HomeAssistantGenerated;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetDaemon.HassModel.Entities;

[NetDaemonApp]
public class OfficeLights
{
    IDynamicLightAdjuster _lightAdjuster;
    LightEntity _officeLights;
    InputBooleanEntity _officeOverride;
    BinarySensorEntity _officeMotion;
    NumericSensorEntity _officeIllumination;

    public OfficeLights(IHaContext ha, Func<IDynamicLightAdjuster.DynamicLightModel, IDynamicLightAdjuster> lightAdjusterFactory)
    {
        _lightAdjuster = lightAdjusterFactory(new IDynamicLightAdjuster.DynamicLightModel(){
            //MinIllumination = 7,
            TargetIllumination = 100,
            MinBrightness = 3,
            MaxLightBrightness = 40,
            IlluminationAddedAtMin = 2,
            IlluminationAddedAtMax = 31
        });

        var _entities = new Entities(ha);
        _officeLights = _entities.Light.OfficeLights;
        _officeOverride = _entities.InputBoolean.OfficeOverride;
        _officeMotion = _entities.BinarySensor.LumiLumiSensorMotionAq2Motion;
        _officeIllumination = _entities.Sensor.LumiLumiSensorMotionAq2Illuminance;

        _entities.Sensor.LumiLumiSensorMotionAq2Illuminance.StateChanges()
            .Subscribe(e => SetState());
        _officeMotion.StateChanges().Subscribe(e => SetState());
    }

    private void SetState()
    {   
        System.Console.WriteLine("enter set state");

        //if the light is off, there is no brightness value
        var currentBrightness = _officeLights.Attributes?.Brightness ?? 0;
        
        bool isOverrideEnabled = _officeOverride.IsOn();
        var currentIllumination = _officeIllumination.State;
        bool isMotion = _officeMotion.IsOn();

        if (isMotion && !isOverrideEnabled && currentIllumination.HasValue )
        {
            System.Console.WriteLine($"Office Illumination {currentIllumination}");

            var newBrightness = _lightAdjuster.GetAppropriateBrightness(currentIllumination.Value, currentBrightness);

            _officeLights.TurnOn(brightness: (long)newBrightness);

            System.Console.WriteLine($"new brightness: {newBrightness}");
        }        
    }
}