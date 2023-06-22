using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public int health;
    public int coins;
    public AudioSource zrodlodzwieku;
    public AudioClip hit;
    public AudioClip heartpick;
    public AudioClip coinpick;
    public AudioClip kill;

    public Image[] hearts;
    public Image coinImage;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite coinSprite;
    public Text coinCountText;
    private bool canTakeDamage = true;

    private float jumpForce = 9f;
    private float backwardForce = -10f;
    private Vector3 initialPosition;

   void Start()
   {
    myRigidbody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    sprite = GetComponent<SpriteRenderer>();
    coinImage.sprite = coinSprite;
    initialPosition = transform.position; // Zapisz pozycję początkową postaci
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
        UpdateHearts();
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }
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




private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Enemy") && canTakeDamage)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 contactNormal = contact.normal;

            // Sprawdź, czy kolizja nastąpiła z góry
            if (contactNormal.y > 0)
            {
                zrodlodzwieku.PlayOneShot(kill);
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                continue; // Jeśli kolizja nastąpiła z góry, przejdź do następnego kontaktu
            }

            // Jeśli kolizja nastąpiła z innej strony niż dół, zmniejsz health
            zrodlodzwieku.PlayOneShot(hit);
            health--;


            // Ustaw flagę canTakeDamage na false, aby zablokować ponowną utratę życia
            canTakeDamage = false;
            StartCoroutine(ResetDamage());

            // Oblicz siłę odrzutu
            Vector2 force = new Vector2(-contactNormal.x, Mathf.Abs(contactNormal.y)).normalized;
            force.y = jumpForce;
            force.x *= backwardForce;
            myRigidbody.velocity = force;

            break; // Przerwij pętlę po znalezieniu pierwszego kontaktu z odpowiednią stroną
        }
    }
}


private IEnumerator ResetDamage()
{
    // Wyłącz możliwość otrzymywania obrażeń
    canTakeDamage = false;

    // Cykliczne włączanie i wyłączanie sprite'a
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    float duration = 2f; // Czas trwania migania (2 sekundy)
    float blinkTime = 0.2f; // Czas trwania jednego mignięcia (0.2 sekundy)
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        // Wyłącz sprite
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(blinkTime);

        // Włącz sprite
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(blinkTime);

        elapsedTime += blinkTime * 2f; // Przesuń czas o długość jednego cyklu migania
    }

    // Włącz możliwość otrzymywania obrażeń
    canTakeDamage = true;
}





        void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D kolizja)
    {
        if (kolizja.gameObject.CompareTag("Reset"))
        {
            // Przenieś postać do punktu początkowego
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 9f);
            StartCoroutine(ResetCharacterPosition());
            health--;
            zrodlodzwieku.PlayOneShot(hit);
        }
        if (kolizja.gameObject.CompareTag("Coin"))
    {
        AddCoin(1); // Dodaj 1 monetę
        kolizja.gameObject.SetActive(false);
    }
    if (kolizja.gameObject.CompareTag("Heart"))
    {
        AddHeart(1); // Dodaj serce
        kolizja.gameObject.SetActive(false);
    }
    }
    public void AddCoin(int ilePunktowDodac)
    {
        coins = coins + ilePunktowDodac;
        print("Dodano " + ilePunktowDodac + " monet...");
        coinCountText.text = 'x'+coins.ToString();
        zrodlodzwieku.PlayOneShot(coinpick);
    }
    public void AddHeart(int ilePunktowDodac)
    {
        health = health + ilePunktowDodac;
        if (health > 3){
            health = 3;
            zrodlodzwieku.clip = heartpick;
            zrodlodzwieku.Play();
        }
        print("Dodano życie");
    }
private IEnumerator ResetCharacterPosition()
{
    yield return new WaitForSeconds(0.55f);

    // Przenieś postać do punktu początkowego
    transform.position = initialPosition;
}




}