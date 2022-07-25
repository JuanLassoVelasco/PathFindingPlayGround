using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isExplored = false;
    public Waypoint exploredFrom;
    public float exploreWeight;

    [SerializeField] Material UnexploredMaterial = null;
    [SerializeField] Material ExploredMaterial = null;
    [SerializeField] Material PathMaterial = null;

    Vector2Int gridPos;
    MeshRenderer waypointRenderer;

    const int gridSize = 10;

    private void Awake()
    {
        waypointRenderer = GetComponent<MeshRenderer>();
    }

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2Int GetGridPosition()
    {
        gridPos.x = Mathf.RoundToInt(transform.position.x / gridSize);
        gridPos.y = Mathf.RoundToInt(transform.position.y / gridSize);

        return gridPos;
    }

    public void SetExplored()
    {
        waypointRenderer.material = ExploredMaterial;
    }

    public void SetUnexplored()
    {
        waypointRenderer.material = UnexploredMaterial;
    }

    public void SetPath()
    {
        waypointRenderer.material = PathMaterial;
    }
}
