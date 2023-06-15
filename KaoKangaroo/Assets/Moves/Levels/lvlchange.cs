using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvlchange : MonoBehaviour
{
    public bool levelchange;
    public int index;
    private Vector3 initialPosition;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            if (levelchange == true)
            {
                // Level by Index
                SceneManager.LoadScene(index);
            }
            else
            {
                // Reset Player Position
            }
        }
    }
}
