using UnityEngine;

public class CrankSystem : MonoBehaviour, IPowerProducer, IInteractable
{
    public int Produce => 1;
    public PowerStorage PowerStorage;

    public void Interact()
    {
        Debug.Log(PowerStorage);
        PowerStorage.Store(Produce);
    }
}
