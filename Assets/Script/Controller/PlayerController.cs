using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour, IDataPersistence
{
    public static int CollectedCandy = 0;

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5f;

    public GameObject Aim;

    public static Vector2 lastMoveDirection;
    private Vector2 moveInput;
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

        if (moveInput != Vector2.zero)
        {
            Vector3 vector3 = Vector3.left * moveInput.x + Vector3.down * moveInput.y;
            Aim.transform.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
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
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.transform.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (HotbarController.EquippedItem != null)
            {
                var item = HotbarController.EquippedItem.GetComponent<Item>();
                item.Use();
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
        transform.position = data.playerPosition;
        Debug.Log("Loaded player position: " + data.playerPosition);

        CollectedCandy =  data.collectedCollectibles.Where(pair => pair.Value == true).Count();
        Debug.Log($"Loaded collected candy: {CollectedCandy}");
    }

    void IDataPersistence.SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
        Debug.Log("Saved player position: " + data.playerPosition);
    }
}
