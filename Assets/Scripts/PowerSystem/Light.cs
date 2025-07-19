using UnityEngine;

public class Light : MonoBehaviour, IPowerConsumer
{
    public int Consume => 1;
    public int lightRadius = 1;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f); // semi-transparent orange
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }

}
