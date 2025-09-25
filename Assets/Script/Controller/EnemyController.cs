using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 2f;

    public enum StateMachine
    {
        Patrol,
        Pursue,
        Stunned
    }

    public StateMachine currentState;

    private bool isFacingRight = true;
    private bool hasLineOfSight = false;
    private float range = 3f;
    private float delay = 0;

    public LayerMask layerMask;

    public Node idleNode;
    public Node currentNode;
    public List<Node> path = new();

    public bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        currentState = StateMachine.Patrol;
        currentNode = AStarManager.instance.GetNearestNode(transform.position);
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
                currentState = StateMachine.Patrol;
            }
            else
            {
                // Keep the enemy stunned
                body.linearVelocity = Vector2.zero;
                return;
            }
        }

        if (isFacingRight && body.linearVelocity.x < 0 || !isFacingRight && body.linearVelocity.x > 0)
        {
            FlipBody();
        }

        switch (currentState)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Pursue:
                Pursue();
                break;
            case StateMachine.Stunned:
                break;
        }

        if (isStunned && currentState != StateMachine.Stunned)
        {
            currentState = StateMachine.Stunned;
        }
        else if (hasLineOfSight && currentState != StateMachine.Pursue)
        {
            currentState = StateMachine.Pursue;
            path.Clear();
        }
        else if (!hasLineOfSight && currentState != StateMachine.Patrol)
        {
            delay = 2f;
            currentState = StateMachine.Patrol;
        }
    }

    public void Stunned(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        body.linearVelocity = Vector2.zero;
        Debug.Log("Enemy stunned");
    }

    private void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, range, layerMask);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");

            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }

    private void Patrol()
    {
        if (path != null && path.Count > 0)
        {
            Move();
            return;
        }

        if (currentNode != idleNode)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
                return;
            }

            CreatePath(currentNode, idleNode);
            Move();
        }
    }

    private void Pursue()
    {
        Node playerNode = AStarManager.instance.GetNearestNode(player.transform.position);
        
        if (currentNode != playerNode)
        {
            CreatePath(currentNode, playerNode);
            Move();
        }
    }

    private void Move()
    {
        if (path != null && path.Count > 0)
        {
            int x = 0;
            body.linearVelocity = (path[x].transform.position - transform.position).normalized * speed;

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }

    private void CreatePath(Node start, Node end)
    {
        if (path == null || path.Count == 0)
            path = AStarManager.instance.GeneratePath(start, end);
        {
        }
    }

    private void FlipBody()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = animator.transform.localScale;
        localScale.x *= -1;
        animator.transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Game Over");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
