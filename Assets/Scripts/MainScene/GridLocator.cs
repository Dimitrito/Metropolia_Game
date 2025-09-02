using UnityEngine;

public class GridLocator : MonoBehaviour
{
    private GridManager _chosenGrid;
    private int _currentX, _currentZ;

    void Start()
    {
        GridManager[] allGrids = FindObjectsOfType<GridManager>();

        if (allGrids.Length == 0)
        {
            Debug.LogError("GridManager не найден!");
            enabled = false;
            return;
        }

        FindNearestFreeCell(allGrids, out _chosenGrid, out _currentX, out _currentZ);

        if (_chosenGrid == null)
        {
            Debug.LogWarning("Свободные клетки не найдены!");
            return;
        }

        RotateObject(_chosenGrid.gameObject.tag);
        transform.position = GetCellCenter(_chosenGrid, _currentX, _currentZ);

        _chosenGrid.PlaceObject(_currentX, _currentZ);
    }

    void RotateObject(string gridTag) 
    {
        switch (gridTag) 
        {
            case "Up":
                transform.Rotate(0, -270, 0);
                break;
            case "Down":
                transform.Rotate(0, 270, 0);
                break;
            case "Right":
                transform.Rotate(0, 180, 0);
                break;
        }
    }

    private void FindNearestFreeCell(GridManager[] grids, out GridManager bestGrid, out int bestX, out int bestZ)
    {
        bestGrid = null;
        bestX = 0;
        bestZ = 0;
        float minDist = Mathf.Infinity;

        foreach (var grid in grids)
        {
            for (int x = 0; x < grid.width; x++)
            {
                for (int z = 0; z < grid.height; z++)
                {
                    if (grid.IsCellFree(x, z))
                    {
                        Vector3 cellCenter = GetCellCenter(grid, x, z);
                        float dist = Vector3.Distance(transform.position, cellCenter);

                        if (dist < minDist)
                        {
                            minDist = dist;
                            bestGrid = grid;
                            bestX = x;
                            bestZ = z;
                        }
                    }
                }
            }
        }
    }

    private Vector3 GetCellCenter(GridManager grid, int x, int z)
    {
        Vector3 origin = grid.transform.position;
        return origin + new Vector3(x * grid.cellSize, 0, z * grid.cellSize);
    }

    public void DeleteObject()
    {
        if (_chosenGrid != null)
        {
            _chosenGrid.grid[_currentX, _currentZ].occupied = false;
        }

        Destroy(gameObject);
    }
}
