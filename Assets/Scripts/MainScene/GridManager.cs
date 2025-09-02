using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 2f;

    public bool showGizmos = true;
    public Color freeCellColor = Color.green;
    public Color occupiedCellColor = Color.red;

    public GridCell[,] grid;

    void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                grid[x, z] = new GridCell();
                grid[x, z].occupied = false;
            }
        }
    }

    public bool IsCellFree(int x, int z)
    {
        return !grid[x, z].occupied;
    }

    public void PlaceObject(int x, int z)
    {
        grid[x, z].occupied = true;
    }

    public void GetCellFromPosition(Vector3 position, out int x, out int z)
    {
        x = Mathf.RoundToInt((position.x - transform.position.x) / cellSize);
        z = Mathf.RoundToInt((position.z - transform.position.z) / cellSize);
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        if (grid == null)
        {
            grid = new GridCell[width, height];
            for (int x = 0; x < width; x++)
                for (int z = 0; z < height; z++)
                    grid[x, z] = new GridCell();
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Gizmos.color = grid[x, z].occupied ? occupiedCellColor : freeCellColor;
                Vector3 cellPos = transform.position + new Vector3(x * cellSize, 0.1f, z * cellSize);
                Gizmos.DrawWireCube(cellPos, new Vector3(cellSize, 0, cellSize));
            }
        }
    }
}

[System.Serializable]
public class GridCell
{
    public bool occupied;
}
