using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Vector2Int size = new Vector2Int(10, 10);
    [SerializeField] private int walkLength = 50;

    [Header("Seed Control")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int seed = 0;

    public GameObject prefab;
    public Dictionary<Vector3Int, bool> gridLights = new Dictionary<Vector3Int, bool>();

    private Camera cam;
    private Grid grid;

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

        gridLights.Clear();

        int finalSeed = useRandomSeed ? DateTime.Now.GetHashCode() : seed;
        var rand = new System.Random(finalSeed);
        Debug.Log("Using Seed: " + finalSeed);

        var visitedTiles = new HashSet<Vector3Int>();
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 0, -1)
        };

        Vector3Int current = new Vector3Int(0, 0, 0);
        visitedTiles.Add(current);

        for (int i = 0; i < walkLength; i++)
        {
            Vector3Int dir = directions[rand.Next(directions.Length)];
            Vector3Int next = current + dir;

            if (next.x >= 0 && next.x < size.x && next.z >= 0 && next.z < size.y)
            {
                current = next;
                visitedTiles.Add(current);
            }
        }

        foreach (var coordinate in visitedTiles)
        {
            var position = grid.GetCellCenterWorld(coordinate);
            GridCell gridCell = Instantiate(prefab, new Vector3(position.x, 0, position.z), Quaternion.identity, transform)
                .AddComponent<GridCell>();

            gridLights[coordinate] = false;
        }
    }
}
