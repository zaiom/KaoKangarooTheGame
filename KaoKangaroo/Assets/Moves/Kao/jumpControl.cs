using UnityEngine;

public class jumpControl : MonoBehaviour
{
    public float jumpForce;
    public float checkRadius;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canJump;
    private PolygonCollider2D playerCollider;
    private float defaultFriction;
    private float slipperyFriction = 0f;

    void Start()
    {
        playerCollider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        canJump = true;
        defaultFriction = Physics2D.defaultContactOffset;
    }

    void Update()
    {
        // Sprawdza czy postać stoi na podłożu
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius, groundLayer);
        isGrounded = IsGrounded();

        // Ograniczenie do jednego skoku, gdy postać jest na ziemi
        if (isGrounded)
        {
            canJump = true;
            SetColliderFriction(defaultFriction);
        }
        else
        {
            SetColliderFriction(slipperyFriction);
        }

        // Skok po wciśnięciu przycisku i gdy postać stoi na podłożu i może skakać
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            jump();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private bool IsGrounded()
    {
        // Pobierz granice collidera gracza
        Bounds bounds = playerCollider.bounds;

        // Określ szerokość i wysokość boxa
        float boxWidth = bounds.size.x - 0.1f; // pomniejsz szerokość boxa o 0.1f z obu stron, aby uniknąć kolizji ze ścianami
        float boxHeight = 0.1f; // wysokość boxa

        // Określ punkt początkowy i kierunek boxa
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y); // punkt początkowy to środek dolnej krawędzi collidera
        Vector2 direction = Vector2.down; // kierunek to w dół

        // Sprawdź, czy w obszarze styku z ziemią znajdują się obiekty z ustawioną warstwą groundLayer
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(boxWidth, boxHeight), 0f, direction, 0.1f, groundLayer);
        return hit.collider != null;
    }

    private void SetColliderFriction(float friction)
    {
        PhysicsMaterial2D material = new PhysicsMaterial2D();
        material.friction = friction;
        playerCollider.sharedMaterial = material;
    }

    public void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false;
    }
}
