using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        Animate();
        if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0)
        {
            FlipBody();
        }

        //FlipBody();

        //float xInput = Input.GetAxis("Horizontal");
        //float yInput = Input.GetAxis("Vertical");

        //if (Mathf.Abs(xInput) > 0)
        //{
        //    float xVelocity = xInput * speed;

        //    body.linearVelocity = new Vector2(xVelocity, body.linearVelocity.y);
        //    moveInput = new Vector2(xVelocity, body.linearVelocity.y);
        //}

        //if (Mathf.Abs(yInput) > 0)
        //{
        //    float yVelocity = yInput * speed;

        //    body.linearVelocity = new Vector2(body.linearVelocity.x, yVelocity);
        //    moveInput = new Vector2(body.linearVelocity.x, yVelocity);
        //}

        //animator.SetFloat("xVelocity", Mathf.Abs(moveInput.magnitude));
    }

    private void FixedUpdate()
    {
        body.linearVelocity = moveInput * speed;
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if ((moveX == 0 && moveY == 0) && (moveInput.x != 0 || moveInput.y != 0))
        {
            lastMoveDirection = moveInput;
        }

        moveInput.x = moveX;
        moveInput.y = moveY;

        moveInput.Normalize();
    }

    void Animate()
    {
        animator.SetFloat("xVelocity", moveInput.x);
        animator.SetFloat("yVelocity", moveInput.y);
        animator.SetFloat("lastPosX", lastMoveDirection.x);
        animator.SetFloat("lastPosY", lastMoveDirection.y);
        animator.SetFloat("MoveMagnitude", moveInput.magnitude);
    }

    void FlipBody()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = animator.transform.localScale;
        localScale.x *= -1;
        animator.transform.localScale = localScale;
    }
}
