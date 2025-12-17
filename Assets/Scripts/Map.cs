using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject _sourceNodePrefab;
    [SerializeField] private GameObject _marketNodePrefab;
    [SerializeField] private GameObject _obstacleNodePrefab;
    [SerializeField] private GameObject _linkNodePrefab;
    [SerializeField] private Vector2Int _gridSize = new (10, 10);
    [SerializeField] private float _cellSize = 1.0f;

    Node[,] nodeGrid;
    int nodeCount;

    public int Width => (int)(_gridSize.x * _cellSize);
    public int Height => (int)(_gridSize.y * _cellSize);
    public Vector2 Center => new (_gridSize.x * _cellSize / 2, _gridSize.y * _cellSize / 2);

    public void Create()
    {
        nodeGrid = new Node[_gridSize.x, _gridSize.y];
        nodeCount = 0;
    }

    public void CreateNodes()
    {
        var location = new Vector2Int(3, 4);
        var position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
        var sourceNode = Instantiate(_sourceNodePrefab, position, Quaternion.identity);
        SetNodeAt(location, sourceNode.GetComponent<Node>());

        location = new Vector2Int(7, 4);
        position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
        var marketNode = Instantiate(_marketNodePrefab, position, Quaternion.identity);
        SetNodeAt(location, marketNode.GetComponent<Node>());

        var obstacleCount = 5;
        for (int i = 0; i < obstacleCount && i < _gridSize.x * _gridSize.y; i++)
        {
            location = new Vector2Int(Random.Range(0, _gridSize.x), Random.Range(0, _gridSize.y));
            if (HasNodeAt(location))
            {
                i--;
                continue;
            }
            position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
            var obstacleNode = Instantiate(_obstacleNodePrefab, position, Quaternion.identity);
            SetNodeAt(location, obstacleNode.GetComponent<Node>());
        }
    }

    public Vector2Int ToLocation(Vector2 position)
    {
        var x = Mathf.RoundToInt(position.x / _cellSize);
        var y = Mathf.RoundToInt(position.y / _cellSize);
        return new Vector2Int(x, y);
    }

    public bool IsInBounds(Vector2Int location)
    {
        return location.x >= 0 && location.x < _gridSize.x && location.y >= 0 && location.y < _gridSize.y;
    }

    public bool HasNodeAt(Vector2Int location)
    {
        if (!IsInBounds(location))
        {
            Debug.LogError("Map: HasNodeAt - Location out of bounds: " + location);
            return false;
        }

        return nodeGrid[location.x, location.y] != null;
    }

    public Node GetNodeAt(Vector2Int location)
    {
        if (!IsInBounds(location))
        {
            Debug.LogError("Map: GetNodeAt - Location out of bounds: " + location);
            return null;
        }

        return nodeGrid[location.x, location.y];
    }

    public void SetNodeAt(Vector2Int location, Node node)
    {
        if (!IsInBounds(location))
        {
            Debug.LogError("Map: SetNodeAt - Location out of bounds: " + location);
            return;
        }

        if (nodeGrid[location.x, location.y] != null)
        {
            nodeCount--;
        }
        nodeGrid[location.x, location.y] = node;
        if (node != null)
        {
            nodeCount++;
        }
    }

    public void LocateLink(List<Vector2Int> locations)
    {
        var startingNode = GetNodeAt(locations[0]);
        for (int i = 1; i < locations.Count; i++)
        {
            var nodeLocation = locations[i];
            var node = GetNodeAt(nodeLocation);
            if (node == null)
            {
                var linkNodePosition = new Vector3(nodeLocation.x * _cellSize, nodeLocation.y * _cellSize, 0);
                var linkNodeObject = Instantiate(_linkNodePrefab, linkNodePosition, Quaternion.identity);
                var linkNode = linkNodeObject.GetComponent<LinkNode>();
                linkNode.transportLines.Add(startingNode.transportLines[0]);
                SetNodeAt(nodeLocation, linkNode);
            }
        }
    }
}