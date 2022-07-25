using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstrasPathFinder : MonoBehaviour
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
                waypoint.exploreWeight = Mathf.Infinity;
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
        startNode.exploreWeight = 0;

        exploreQueue.Enqueue(startPosition);

        while (exploreQueue.Count > 0 && !goalFound)
        {
            currentPosition = exploreQueue.Dequeue();

            currentNode = grid[currentPosition];

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
                    if (newNeighbor.GetGridPosition() == goalPosition) { goalFound = true; }

                    if (!exploreQueue.Contains(searchPosition)) { exploreQueue.Enqueue(searchPosition); }

                    if ((currentNode.exploreWeight + 1) < newNeighbor.exploreWeight)
                    {
                        newNeighbor.exploreWeight = currentNode.exploreWeight + 1;
                        newNeighbor.exploredFrom = currentNode;
                    }
                }
            }

            currentNode.SetExplored();
            currentNode.isExplored = true;
            yield return new WaitForSeconds(runSpeedSeconds);
        }

        if (goalFound || !(goalNode.exploreWeight >= Mathf.Infinity))
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
