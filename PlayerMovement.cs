using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MOVE_SPEED = 5f;
    private Vector2 direction;
    private GameObject PLAYER;
    public new Rigidbody2D rigidbody2D;
    private PlayerControl playerControl;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PLAYER = GameObject.FindGameObjectWithTag("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerControl = GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FixRotation();
    }

    private void FixedUpdate()
    {
        rigidbody2D.linearVelocity = direction * MOVE_SPEED;
    }
    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isMoving", true);
        if (context.canceled)
        {
            animator.SetFloat("LastX", direction.x);
            animator.SetFloat("LastY", direction.y);
            animator.SetBool("isMoving", false);
        }
        direction = context.ReadValue<Vector2>();
        animator.SetFloat("DirectX", direction.x);
        animator.SetFloat("DirectY", direction.y);
    }

    private void FixRotation()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdateAnimation()
    {
       
    }
}
