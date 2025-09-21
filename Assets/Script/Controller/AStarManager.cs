using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();

        foreach (Node n in FindObjectsByType<Node>(FindObjectsSortMode.None))
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector3.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;
            
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                List<Node> path = new();

                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.cameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach (Node connectedNode in currentNode.connection)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (heldGScore < connectedNode.gScore)
                {
                    connectedNode.cameFrom = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }

    public Node GetNearestNode(Vector2 position)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach(Node node in NodeInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);
            
            if (currentDistance < minDistance)
            {
                foundNode = node;
                minDistance = currentDistance;
            }
        }

        return foundNode;
    }

    public Node GetFurthestNode(Vector2 position)
    {
        Node foundNode = null;
        float maxDistance = 0;

        foreach (Node node in NodeInScene())
        {
            float currentDistance = Vector2.Distance(position, node.transform.position);

            if (currentDistance > maxDistance)
            {
                foundNode = node;
                maxDistance = currentDistance;
            }
        }

        return foundNode;
    }

    public Node[] NodeInScene()
    {
        return FindObjectsByType<Node>(FindObjectsSortMode.None);
    }
}
