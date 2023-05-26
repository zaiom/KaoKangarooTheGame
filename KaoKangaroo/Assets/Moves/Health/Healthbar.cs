using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public int health;
    public int coins;

    public Image[] hearts;
    public Image coinImage;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite coinSprite;

    public Text coinCountText;
    void Start()
    {
        coinImage.sprite = coinSprite;
    }
    void Update()
    {
        UpdateHearts();
        
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
        if (kolizja.gameObject.CompareTag("Coin"))
    {
        AddCoin(1); // Dodaj 1 monetę
        kolizja.gameObject.SetActive(false);
    }
    }
    public void AddCoin(int ilePunktowDodac)
    {
        coins = coins + ilePunktowDodac;
        print("Dodano " + ilePunktowDodac + " monet...");
        coinCountText.text = 'x'+coins.ToString();
    }
}
