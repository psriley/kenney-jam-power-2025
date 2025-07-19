using NUnit.Framework;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject Build(GameObject obj, GridCell cell)
    {
        return Instantiate(obj, cell.transform);
    }
}
