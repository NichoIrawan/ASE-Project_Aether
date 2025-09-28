using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 3f;

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

    [ContextMenu("Generate guid")]
    private void GenerateGuid()
    {
        id = Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.Unregister(this);
        }
    }

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
                body.linearVelocity = Vector2.zero;
                StopSound();
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
                StopSound();
                break;
            case StateMachine.Pursue:
                Pursue();
                StartSound();
                break;
            case StateMachine.Stunned:
                break;
        }

        if (isStunned && currentState != StateMachine.Stunned)
        {
            currentState = StateMachine.Stunned;
        }
        else if (!isStunned && hasLineOfSight && currentState != StateMachine.Pursue)
        {
            currentState = StateMachine.Pursue;
            path.Clear();
        }
        else if (!isStunned && !hasLineOfSight && currentState != StateMachine.Patrol)
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
        if (isStunned) return;

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
        {
            path = AStarManager.instance.GeneratePath(start, end);
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

    public void LoadData(GameData data)
    {
        if (data.enemies.TryGetValue(id, out var enemyData))
        {
            transform.position = enemyData.position;
            currentState = enemyData.currentState;
            stunTimer = enemyData.stunTimer;

            currentNode = AStarManager.instance.GetNearestNode(transform.position);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.enemies.ContainsKey(id))
        {
            data.enemies.Remove(id);
        }

        EnemyData enemyData = new(transform.position, currentState, stunTimer);
        data.enemies.Add(id, enemyData);
    }

    private void StartSound()
    {
        InvokeRepeating(nameof(playSound), 0f, 5f);
    }

    private void StopSound()
    {
        CancelInvoke(nameof(playSound));
    }

    private void playSound()
    {
        SoundEffectManager.PlaySound("Enemy");
    }
}
