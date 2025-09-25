using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : Item
{
    public LayerMask enemyLayers;
    public float timeOn = 5f;
    public float stunDuration = 3f;
    public float cooldown = 10f;
    public float range = 5f;

    private float cooldownTimer = 0f;

    private bool isOn = false;
    public FlashlightSpotLight flashlightSpotLight;

    private void Awake()
    {
        flashlightSpotLight.range = range;
        flashlightSpotLight.angle = Quaternion.LookRotation(Vector3.forward, PlayerScript.lastMoveDirection);

        flashlightSpotLight.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public override void Use()
    {
        if (cooldownTimer > 0)
        {
            Debug.Log("Flashlight is on cooldown for " + cooldownTimer.ToString("F1") + " seconds.");
            return;
        }

        if (isOn) return;
        
        StartCoroutine(FlashlightSequence());
    }

    private IEnumerator FlashlightSequence()
    {
        // Activate flashlight
        flashlightSpotLight.angle = Quaternion.LookRotation(Vector3.forward, PlayerScript.lastMoveDirection);

        isOn = true;
        cooldownTimer = cooldown;
        flashlightSpotLight.gameObject.SetActive(true);
        Debug.Log("Flashlight is ON");

        yield return new WaitForSeconds(timeOn);

        // Deactivate after timeOn run out
        isOn = false;
        flashlightSpotLight.gameObject.SetActive(false);
        Debug.Log("Flashlight is OFF");
    }
}
