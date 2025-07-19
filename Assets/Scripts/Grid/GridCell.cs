using UnityEngine;

public class GridCell : MonoBehaviour, IInteractable
{
    // Does this cell have a light on it
    public bool isOccupied = false;
    private GameObject occupant;

    public void Interact()
    {
        if (!occupant)
        {
            // PlaceLight();
        }
    }
}
