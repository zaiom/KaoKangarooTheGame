using UnityEngine;

public class CollisionFix : MonoBehaviour
{
    public int collisionIterations = 10;
    public float timeStep = 0.02f;

    private void FixedUpdate()
    {
        for (int i = 0; i < collisionIterations; i++)
        {
            Physics.Simulate(timeStep);
        }
    }
}
