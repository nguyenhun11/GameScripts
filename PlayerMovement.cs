using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MOVE_SPEED = 5f;
    private bool canMove;
    private Vector2 direction;
    private new Rigidbody2D rigidbody2D;

    private Animator animator;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    void Update()
    {
        FixRotation();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rigidbody2D.linearVelocity = direction * MOVE_SPEED;
        }
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

    public void StopMove()
    {
        canMove = false;
    }

    public void CanMove()
    {
        canMove = true;
    }

}
