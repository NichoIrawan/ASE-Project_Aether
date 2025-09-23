using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : Item
{
    public LayerMask enemyLayers;
    public float timeOn = 5f;
    public float stunDuration = 3f;
    public float cooldown = 10f;

    private float timer = 0f;

    private bool isOn = false;
    public GameObject flashlightLightPrefab;

    private void Awake()
    {
        base.range = 5f;

        var light = flashlightLightPrefab.GetComponent<FlashlightSpotLight>();
        light.range = base.range;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void Use()
    {
        if (timer > 0)
        {
            Debug.Log("Flashlight is on cooldown for " + timer.ToString("F1") + " seconds.");
            return;
        }
        else
        {
            timer = cooldown;
            isOn = true;

            // Activate the flashlight light


            // Stun enemies in range
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                var enemyComponent = enemy.GetComponent<EnemyController>();
                if (enemyComponent != null)
                {
                    enemyComponent.Stunned(stunDuration);
                }
            }

            Debug.Log("Flashlight is now " + (isOn ? "ON" : "OFF"));
        }

    }
}
