using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour, IDataPersistence
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
        //ProcessInput();
        Animate();
        if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0)
        {
            FlipBody();
        }
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

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (HotbarController.EquippedItem != null)
            {
                HotbarController.EquippedItem.GetComponent<Item>().Use();
            }
        }
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

    void IDataPersistence.LoadData(GameData data)
    {
        transform.position = data.position;
        Debug.Log("Loaded player position: " + data.position);
    }

    void IDataPersistence.SaveData(ref GameData data)
    {
        data.position = transform.position;
        Debug.Log("Saved player position: " + data.position);
    }
}
