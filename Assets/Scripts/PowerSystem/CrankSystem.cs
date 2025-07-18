using UnityEngine;

public class CrankSystem : MonoBehaviour, IPowerProducer, IInteractable
{
    public int Produce => 1;
    public PowerStorage PowerStorage;

    public void Interact()
    {
        Debug.Log("Interacted with Crank System");
        PowerStorage.Store(Produce);
    }
}
