using UnityEngine;

public class FlashlightTriggerController : MonoBehaviour
{
    public float range = 5f;
    public float stunDuration = 3f;
    public LayerMask enemyLayer;

    [Range(1, 180)]
    public float coneAngle = 60f;

    [SerializeField]private CircleCollider2D detectionCollider;

    void Update()
    {
        // Ensures the collider radius always matches the public 'range' variable
        if (detectionCollider.radius != range)
        {
            detectionCollider.radius = range;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Filter by layer first
        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
        {
            return;
        }

        // Check if the enemy is within the cone's angle
        Vector2 directionToTarget = (other.transform.position - transform.position).normalized;
        Vector2 forwardDirection = PlayerScript.lastMoveDirection;

        if (Vector2.Angle(forwardDirection, directionToTarget) < coneAngle / 2)
        {
            // 3. Get the component and apply the stun
            var enemyComponent = other.GetComponentInParent<EnemyController>();
            if (enemyComponent != null)
            {
                enemyComponent.Stunned(stunDuration);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (detectionCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, detectionCollider.radius);
        }
    }
}