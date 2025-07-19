using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;

public class GridGenerator : MonoBehaviour
{
    public GameObject prefab;
    public Vector2Int size = new Vector2Int(10, 10);

    private Camera cam;
    private Grid grid;

    public Dictionary<Vector3Int, bool> gridLights = new Dictionary<Vector3Int, bool>();

    // private Vector3 cameraPositionTarget;
    // private float cameraSizeTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GetComponent<Grid>();
        cam = Camera.main;
        Generate();
    }

    private void Generate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var coordinates = new List<Vector3Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                coordinates.Add(new Vector3Int(x, 0, y));
                gridLights[new Vector3Int(x, 0, y)] = false;
            }
        }

        var bounds = new Bounds();
        var index = 0;
        var rand = new Random(420);

        foreach (var coordinate in coordinates)
        {
            var position = grid.GetCellCenterWorld(coordinate);
            GridCell gridCell = Instantiate(prefab, new Vector3(position.x, 0, position.z), Quaternion.identity, transform).AddComponent<GridCell>();
        }
    }
}
