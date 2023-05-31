using UnityEngine;

public class DiedAlgorytm : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gracz wskoczył na obiekt!");
        }
    }
}
