using NUnit.Framework;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public void Build(GameObject obj, GridCell cell)
    {
        Instantiate(obj, cell.transform);
    }
}
