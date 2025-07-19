using UnityEngine;

public class FogTile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            Reveal();
        }
    }

    private void Reveal()
    {
        Destroy(gameObject);
    }
}
