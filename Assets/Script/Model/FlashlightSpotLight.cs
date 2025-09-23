using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightSpotLight : MonoBehaviour
{
    [SerializeField] private Light2D spotlight;
    public float range;
    public Quaternion angle;

    private void Awake()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light2D>();
        }

        spotlight.pointLightOuterRadius = range;
        spotlight.pointLightInnerRadius = range * 0.5f;
        spotlight.transform.rotation = angle;
    }
}
