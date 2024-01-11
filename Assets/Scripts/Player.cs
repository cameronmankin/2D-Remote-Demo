using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    public float RunSpeed;
    public float JumpSpeed;
    public LayerMask GroundMask;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInputs = Input.GetAxis("Horizontal");
        
        rigidbody.velocity = new Vector2 (RunSpeed * horizontalInputs, rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fruit potentialFruit = collision.gameObject.GetComponent<Fruit>();

        if (potentialFruit != null)
        {
            Destroy(potentialFruit.gameObject);
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundMask);
        return isGrounded;
    }
}
