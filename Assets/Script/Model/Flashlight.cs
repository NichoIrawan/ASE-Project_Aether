using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : Item
{
    public LayerMask enemyLayers;
    public float timeOn = 5f;
    public float stunDuration = 3f;
    public float cooldown = 10f;

    private float cooldownTimer = 0f;

    private bool isOn = false;
    public FlashlightSpotLight flashlightSpotLight;

    private void Awake()
    {
        base.range = 5f;

        flashlightSpotLight.range = base.range;
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

        if (isOn)
        {
            // This prevents using the flashlight again while it's already on
            return;
        }

        // Start the coroutine to handle the flashlight's active state
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

        // Stun enemy
        Vector2 direction = PlayerScript.lastMoveDirection;
        float coneAngle = 60f; 

        Collider2D[] allEnemiesInRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayers);
        foreach (var enemy in allEnemiesInRange)
        {
            Vector2 vectorToEnemy = (enemy.transform.position - transform.position).normalized;

            // Check if the enemy is within the flashlight's cone
            if (Vector2.Angle(direction, vectorToEnemy) < coneAngle / 2)
            {
                var enemyComponent = enemy.GetComponent<EnemyController>();
                if (enemyComponent != null)
                {
                    enemyComponent.Stunned(stunDuration);
                }
            }
        }

        // --- Wait Phase ---
        yield return new WaitForSeconds(timeOn);

        // --- Deactivation Phase ---
        isOn = false;
        flashlightSpotLight.gameObject.SetActive(false);
        Debug.Log("Flashlight is OFF");
    }
}
