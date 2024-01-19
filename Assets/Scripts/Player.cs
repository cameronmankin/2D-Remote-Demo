using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum MovementState
    {
        Idle = 0,
        Run = 1,
        Jump = 2
    }

    // Variables
    public float RunSpeed;
    public float JumpSpeed;
    public LayerMask GroundMask;
    public Player PlayerPrefab;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInputs = Input.GetAxis("Horizontal");

        _rigidbody.velocity = new Vector2(RunSpeed * horizontalInputs, _rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpSpeed);

            Player instance = Instantiate(PlayerPrefab);
        }

        UpdateAnimation(horizontalInputs);

        if(transform.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fruit potentialFruit = collision.gameObject.GetComponent<Fruit>();

        if (potentialFruit != null)
        {
            Destroy(potentialFruit.gameObject);
        }

        Exit potentialExit = collision.gameObject.GetComponent<Exit>();

        if (potentialExit != null)
        {
            int toLoad = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(toLoad);
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundMask);
        return isGrounded;
    }

    private void UpdateAnimation(float horizontalInput)
    {
        MovementState currentState;

        if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
            
        }
        else if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;   
        }

        if (!IsGrounded())
        {
            currentState = MovementState.Jump;
        }
        else if (horizontalInput != 0)
        {
            currentState = MovementState.Run;
        }
        else
        {
            currentState = MovementState.Idle;
        }

        _animator.SetInteger("MovementState", (int)currentState);

    }
}
