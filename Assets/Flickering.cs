using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flickering : MonoBehaviour
{

    [SerializeField] private Light2D light;
    [SerializeField] private float maxRadious;
    [SerializeField] private float minRadious;
    [SerializeField] private float pace;
    IEnumerator Start()
    {
        var wait = new WaitForSecondsRealtime(0.1f);
        while (true)
        {
            if (light.pointLightOuterRadius >= maxRadious || light.pointLightOuterRadius <= minRadious)
                pace = -pace;

            light.pointLightOuterRadius += pace * Time.deltaTime;
            yield return null;
        }
    }
}
