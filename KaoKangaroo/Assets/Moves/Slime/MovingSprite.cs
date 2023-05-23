using UnityEngine;

public class MovingSprite : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float changeDirInterval = 2f;
    public SpriteRenderer spriteRenderer;

    private float timeSinceLastDirChange = 0f;
    private bool isFacingRight = false;

    void FixedUpdate()
    {
        // move the sprite horizontally
        float moveHorizontal = isFacingRight ? 1f : -1f;
        transform.Translate(moveHorizontal * moveSpeed * Time.deltaTime, 0f, 0f);

        // flip the sprite if necessary
        if ((moveHorizontal > 0 && !isFacingRight) || (moveHorizontal < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // change direction every changeDirInterval seconds
        timeSinceLastDirChange += Time.deltaTime;
        if (timeSinceLastDirChange > changeDirInterval)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            timeSinceLastDirChange = 0f;
        }
    }
}
