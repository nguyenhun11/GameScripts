using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MOVE_SPEED = 5f;
    private Vector2 direction;
    private new Rigidbody2D rigidbody2D;
    private bool isPlayingWalk = false;
    public float walkSpeed = 0.5f;

    private Animator animator;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FixRotation();
    }

    private void FixedUpdate()
    {
        if (Controller_Pause.isGamePaused)
        {
            Pause();
            return;
        }
        rigidbody2D.linearVelocity = direction * MOVE_SPEED;
        animator.SetBool("isMoving", rigidbody2D.linearVelocity.magnitude > 0);
        if (rigidbody2D.linearVelocity.magnitude > 0
            && !isPlayingWalk)
        {
            StartWalk();
        }
        else if (rigidbody2D.linearVelocity.magnitude == 0)
        {
            StopWalk();
        }
    }

    public void SetMoveAnimator(InputAction.CallbackContext context)
    {
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

    private void Pause()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        StopWalk();
    }

    private void StartWalk()
    {
        isPlayingWalk = true;
        InvokeRepeating(nameof(PlayWalkSound), 0f, walkSpeed);
    }

    private void StopWalk()
    {
        isPlayingWalk = false;
        CancelInvoke(nameof(PlayWalkSound));
    }

    private void PlayWalkSound()
    {
        SoundEffectManager.Play("Walk", true);
    }
}
