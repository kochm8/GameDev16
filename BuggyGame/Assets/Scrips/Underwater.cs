using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour
{

    public static float underwaterLevel = 4;

    private bool defaultFog = true;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private float defaultStartDistance;

    void Start()
    {
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
        defaultStartDistance = RenderSettings.fogStartDistance;

        GetComponent<Camera>().backgroundColor = new Color(0, 0.4f, 0.7f, 1);
    }

    void Update()
    {

        if (transform.position.y < underwaterLevel)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
            RenderSettings.fogDensity = 0.04f;
            RenderSettings.fogStartDistance = 0.0f;
        }
        else {
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
            RenderSettings.skybox = defaultSkybox;
            RenderSettings.fogStartDistance = defaultStartDistance;
        }
    }
}