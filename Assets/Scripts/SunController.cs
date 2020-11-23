using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SunController: MonoBehaviour
{
    private Light2D Light;

    void Start()
    {
        Light = GetComponentInChildren<Light2D>();
    }

    public void IncreaseIntensity()
    {
        Light.pointLightOuterRadius += 0.1f;
        Light.intensity += 0.1f;
    }
}