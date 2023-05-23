using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Move : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer sprite;
    [SerializeField] private float MoveSpeed = 7f;
    private float dirX = 0f;
    private enum MovementState {stay,running,jumping,falling}
    private PolygonCollider2D polygonCollider;
    private float previousDirectionX = 1f;
    


   void Start()
   {
    myRigidbody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    sprite = GetComponent<SpriteRenderer>();
   }
    void Update()
    {   
        // Zmiana kierunku postaci podczas trzymania klawisza
        dirX = Input.GetAxisRaw("Horizontal");
        myRigidbody.velocity = new Vector2(dirX*MoveSpeed, myRigidbody.velocity.y);
        
        // Skok po naciśnięciu
        // if (Input.GetButtonDown("Jump"))
        // {
        //     myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 14f);
        // }
        
        AnimationUpdate();
    }

    private void AnimationUpdate()
    {
        MovementState state;
// Zmiana kierunku animacji
        if (dirX > 0)
        {
            state = MovementState.running;
            sprite.flipX = false;
            if (previousDirectionX < 0)
        {
            FlipCollider(false);
        }
        previousDirectionX = 1f;
        }
        else if (dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
            if (previousDirectionX > 0)
        {
            FlipCollider(true);
        }
        previousDirectionX = -1f;
        }

        else
        {
            state = MovementState.stay;
        }
        
        if (myRigidbody.velocity.y > .1f)
        {
            state= MovementState.jumping;

        }
        else if (myRigidbody.velocity.y < -.1f)
        {
           state= MovementState.falling; 
        }
        animator.SetInteger("state", (int)state);
    }


public void FlipCollider(bool isFacingLeft)
{
    // Get the current collider
    PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

    // Flip the collider horizontally
    Vector2[] points = collider.points;
    for (int i = 0; i < points.Length; i++)
    {
        points[i] = new Vector2(-points[i].x, points[i].y);
    }
    collider.points = points;

    // Update the collider offset
    Vector2 offset = collider.offset;
    offset.x = isFacingLeft ? Mathf.Abs(offset.x) : -Mathf.Abs(offset.x);
    collider.offset = offset;
}





}

        
    

 