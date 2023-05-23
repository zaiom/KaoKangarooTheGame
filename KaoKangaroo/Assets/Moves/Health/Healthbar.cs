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

    void Update()
    {
        UpdateHearts();
        UpdateCoins();
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

    void UpdateCoins()
    {
        coinImage.sprite = coinSprite;
        coinCountText.text = 'x'+coins.ToString();
    }
}
