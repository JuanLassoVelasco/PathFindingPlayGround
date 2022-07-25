using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Waypoint startNode = null;
    [SerializeField] Waypoint goalNode = null;
    [SerializeField] float runSpeedSeconds = 0.2f;

    private bool goalFound = false;

    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();

    private void Start()
    {
        LoadGrid();
        StartCoroutine(FindPath());
    }

    private void LoadGrid()
    {
        var waypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in waypoints)
        {
            Vector2Int gridPos = waypoint.GetGridPosition();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Skipping overlaping block" + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
        }
    }

    private IEnumerator FindPath()
    {
        Vector2Int startPosition = startNode.GetGridPosition();
        Vector2Int goalPosition = goalNode.GetGridPosition();
        Vector2Int currentPosition = startPosition;
        Waypoint currentNode = null; // For drawing path when valid path is found

        Queue<Vector2Int> exploreQueue = new Queue<Vector2Int>();

        grid[currentPosition].SetExplored();
        startNode.isExplored = true;

        exploreQueue.Enqueue(startPosition);

        while (exploreQueue.Count > 0 && !goalFound)
        {
            currentPosition = exploreQueue.Dequeue();

            Vector2Int up = currentPosition;
            Vector2Int right = currentPosition;
            Vector2Int down = currentPosition;
            Vector2Int left = currentPosition;

            up.y++;
            right.x++;
            down.y--;
            left.x--;

            Vector2Int[] searchPositions = { up, down, left, right };

            foreach (Vector2Int searchPosition in searchPositions)
            {
                if (grid.ContainsKey(searchPosition) && !grid[searchPosition].isExplored)
                {
                    Waypoint newNeighbor = grid[searchPosition];
                    if (newNeighbor.GetGridPosition() == goalPosition) {
                        goalFound = true;
                    }
                    exploreQueue.Enqueue(searchPosition);
                    newNeighbor.SetExplored();
                    newNeighbor.isExplored = true;
                    newNeighbor.exploredFrom = grid[currentPosition];
                    yield return new WaitForSeconds(runSpeedSeconds);
                }
            }
        }

        if (goalFound)
        {
            currentNode = goalNode;
            goalNode.SetPath();
            while (currentPosition != startPosition)
            {
                currentNode.exploredFrom.SetPath();
                currentNode = currentNode.exploredFrom;
                currentPosition = currentNode.GetGridPosition();
            }
        }
        else
        {
            print("No valid path found to goal node");
        }
    }
}
