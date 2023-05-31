using UnityEngine;

public class MovingSprite : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float changeDirInterval = 2f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Collider2D enemyCollider;

    private enum MovementState { running, dead }
    private MovementState currentState = MovementState.running;
    private float timeSinceLastDirChange = 0f;
    private bool isFacingRight = false;

    private void Start()
    {
        // Inicjalizacja animatora
        animator = GetComponent<Animator>();

        // Dodanie zdarzenia do animacji "Dead"
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Dead")
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = "OnAnimationFinished";
                animationEvent.time = clip.length;
                clip.AddEvent(animationEvent);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentState == MovementState.running)
        {
            // Poruszaj przeciwnikiem poziomo
            float moveHorizontal = isFacingRight ? 1f : -1f;
            transform.Translate(moveHorizontal * moveSpeed * Time.deltaTime, 0f, 0f);

            // Odwróć przeciwnika, jeśli jest to konieczne
            if ((moveHorizontal > 0 && !isFacingRight) || (moveHorizontal < 0 && isFacingRight))
            {
                isFacingRight = !isFacingRight;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            // Zmień kierunek co określony czas
            timeSinceLastDirChange += Time.deltaTime;
            if (timeSinceLastDirChange > changeDirInterval)
            {
                isFacingRight = !isFacingRight;
                spriteRenderer.flipX = !spriteRenderer.flipX;
                timeSinceLastDirChange = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == MovementState.running && collision.gameObject.CompareTag("Player"))
        {
            // Sprawdź, czy górna część collidera przeciwnika ma kolizję z graczem
            Vector2 contactPoint = collision.GetContact(0).point;
            if (enemyCollider.OverlapPoint(contactPoint))
            {
                Die();
            }
        }
    }

    public void Die()
    {
        currentState = MovementState.dead;


        // Uruchom animację "dead" w animatorze
        animator.SetTrigger("Dead");
    }

    private void OnAnimationFinished()
    {
        // Usuń obiekt z hierarchii
        Destroy(gameObject);
    }
}
