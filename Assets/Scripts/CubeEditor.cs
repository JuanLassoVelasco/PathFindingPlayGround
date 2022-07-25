using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{

    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        Vector2Int gridPos = waypoint.GetGridPosition();
        transform.position = new Vector3(gridPos.x * gridSize, gridPos.y * gridSize, 0f);
    }

    private void UpdateLabel()
    {
        Vector2Int gridPos = waypoint.GetGridPosition();
        string gridLabel = (gridPos.x) + "," + (gridPos.y);

        gameObject.name = gridLabel;
    }
}
