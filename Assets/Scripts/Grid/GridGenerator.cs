using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int walkLength = 50;

    [Header("Seed Control")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int seed = 0;

    [Header("Generated Prefabs")]
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject metalTilePrefab;
    [SerializeField] private int metalTileChance = 20;


    public Dictionary<Vector3Int, bool> gridLights = new Dictionary<Vector3Int, bool>();

    private Camera cam;
    private Grid grid;
    private HashSet<Vector3Int> visitedTiles = new HashSet<Vector3Int>();

    // private Vector3 cameraPositionTarget;
    // private float cameraSizeTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GetComponent<Grid>();
        cam = Camera.main;
        Generate();
    }

    private void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gridLights.Clear();
    }

    private int GetSeed()
    {
        int finalSeed = useRandomSeed ? DateTime.Now.GetHashCode() : seed;
        Debug.Log("Using Seed: " + finalSeed);
        return finalSeed;
    }

    private void WalkTiles(System.Random rand)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.forward,
            Vector3Int.back
        };

        Vector3Int current = new Vector3Int(0, 0, 0);
        visitedTiles.Add(current);

        for (int i = 0; i < walkLength; i++)
        {
            Vector3Int dir = directions[rand.Next(directions.Length)];
            Vector3Int next = current + dir;

            current = next;
            visitedTiles.Add(current);
            GenerateTiles(current);
        }
    }

    void GenerateTiles(Vector3Int coordinate)
    {
        var position = grid.GetCellCenterWorld(coordinate);

        // Hey is the chanc
        Random random = new Random();
        int randomNumberInRange = random.Next(101);

        GameObject prefabToGenerate = floorTilePrefab;
        if (randomNumberInRange < metalTileChance)
        {
            prefabToGenerate = metalTilePrefab;
        }

        GridCell gridCell = Instantiate(prefabToGenerate, new Vector3(position.x, 0, position.z), Quaternion.identity, transform)
            .AddComponent<GridCell>();

        gridLights[coordinate] = false;
    }

    private void Generate()
    {
        ClearGrid();

        int randSeed = GetSeed();
        var rand = new System.Random(randSeed);

        WalkTiles(rand);
    }
}
