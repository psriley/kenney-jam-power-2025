using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    // Does this cell have a light on it
    public bool IsOccupied => occupant != null;
    private GameObject occupant;

    public void SetOccupant(GameObject occupant)
    {
        this.occupant = occupant;
    }

    public Vector3 GetOccupantPosition() { 
        return transform.position;
    }
}
