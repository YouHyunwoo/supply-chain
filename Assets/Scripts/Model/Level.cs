using System.Collections.Generic;
using SupplyChain.Enum;
using SupplyChain.Exception;
using SupplyChain.View.World;
using UnityEngine;

namespace SupplyChain.Model
{
    public class Level
    {
        public Data.Level Data;
        public int Index;
        // public string Name;
        // public string Description;
        private readonly List<Vector2Int> _availableLocations = new();
        private TerrainType[,] _terrainTypeGrid;
        private GameObject[,] _terrainObjectGrid;
        private NodeType[,] _nodeTypeGrid;
        private Node[,] _nodeObjectGrid;
        private FogType[,] _fogTypeGrid;
        private GameObject[,] _fogObjectGrid;
        private float _timer, _duration;
        private int _earnings;
        private Transform _levelWorldView;

        public Vector2Int Center => new(Data.Size.x / 2, Data.Size.y / 2);
        public float Duration => _duration;
        public float Timer => _timer;
        public int Earnings => _earnings;

        public void Create()
        {
            ClearAvailableLocations();
            CreateTerrainTypeGrid();
            CreateNodeTypeGrid();
            CreateFogFlagGrid();
        }

        private void ClearAvailableLocations()
        {
            _availableLocations.Clear();
        }

        private Vector2Int SampleAvailableLocation()
        {
            if (_availableLocations.Count == 0)
                throw new LocationNotAvailableException();

            var randomIndex = Random.Range(0, _availableLocations.Count);
            var randomLocation = _availableLocations[randomIndex];
            _availableLocations.RemoveAt(randomIndex);
            return randomLocation;
        }

        private void ReturnAvailableLocation(Vector2Int location)
        {
            if (!_availableLocations.Contains(location))
                _availableLocations.Add(location);
        }

        private void CreateTerrainTypeGrid()
        {
            int width = Data.Size.x;
            int height = Data.Size.y;
            var center = new Vector2Int(width / 2, height / 2);
            var heightThreshold = 0.3f;
            var noise = Random.Range(0f, 100f);

            _terrainTypeGrid = new TerrainType[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Vector2.Distance(new Vector2Int(x, y), center) < Data.InitialSectorSize)
                    {
                        _terrainTypeGrid[x, y] = TerrainType.Empty;
                    }
                    else
                    {
                        var heightValue = Mathf.PerlinNoise((x + noise) * 0.1f, (y + noise) * 0.1f);
                        _terrainTypeGrid[x, y] = heightValue < heightThreshold ? TerrainType.Block : TerrainType.Empty;
                        if (_terrainTypeGrid[x, y] == TerrainType.Empty)
                            _availableLocations.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        private void CreateNodeTypeGrid()
        {
            int width = Data.Size.x;
            int height = Data.Size.y;
            var center = new Vector2Int(width / 2, height / 2);
            var area = width * height;
            var location = Vector2Int.zero;
            var position = Vector3.zero;

            _nodeTypeGrid = new NodeType[width, height];

            _nodeTypeGrid[center.x - 1, center.y] = NodeType.Source;
            _nodeTypeGrid[center.x + 1, center.y] = NodeType.Market;
    
            var veinCount = area * 0.1f;
            for (int i = 0; i < veinCount; i++)
            {
                try
                {
                    location = SampleAvailableLocation();
                }
                catch (LocationNotAvailableException)
                {
                    break;
                }
                position = new Vector3(location.x, location.y);
                _nodeTypeGrid[location.x, location.y] = NodeType.Source;
            }
        }

        private void CreateFogFlagGrid()
        {
            int width = Data.Size.x;
            int height = Data.Size.y;
            var center = new Vector2Int(width / 2, height / 2);
            var sqrInitialSectorSize = Data.InitialSectorSize * Data.InitialSectorSize;

            _fogTypeGrid = new FogType[width, height];

            var location = new Vector2Int();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    location.x = x;
                    location.y = y;
                    var sqrDistance = Vector2.SqrMagnitude(location - center);
                    _fogTypeGrid[x, y] = sqrDistance > sqrInitialSectorSize ? FogType.Foggy : FogType.Cleared;
                }
            }
        }

        public void SetUp()
        {
            _timer = 0f;
            _duration = Data.Duration + Globals.MainSystem.Player.AdditionalTime;
            _earnings = 0;
            _levelWorldView = new GameObject("Level").transform;
            SetUpTerrainObjectGrid();
            SetUpNodeObjectGrid();
            SetUpFogObjectGrid();
        }

        private void SetUpTerrainObjectGrid()
        {
            var width = Data.Size.x;
            var height = Data.Size.y;

            _terrainObjectGrid = new GameObject[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var position = new Vector3(x, y);
                    var terrainType = _terrainTypeGrid[x, y];
                    var terrainObject = Globals.TerrainFactory.CreateTerrainObject(terrainType, position, _levelWorldView.transform);
                    if (terrainObject == null) continue;
                    _terrainObjectGrid[x, y] = terrainObject;
                }
            }
        }

        private void SetUpNodeObjectGrid()
        {
            var width = Data.Size.x;
            var height = Data.Size.y;

            _nodeObjectGrid = new Node[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var position = new Vector3(x, y);
                    var nodeType = _nodeTypeGrid[x, y];
                    try
                    {
                        var nodeObject = Globals.NodeFactory.CreateNodeObject(nodeType, position, _levelWorldView.transform);
                        _nodeObjectGrid[x, y] = nodeObject.GetComponent<Node>();
                    }
                    catch (UnknownNodeTypeException)
                    {
                        continue;
                    }
                }
            }
        }

        public void SetUpFogObjectGrid()
        {
            var width = Data.Size.x;
            var height = Data.Size.y;

            _fogObjectGrid = new GameObject[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var fogType = _fogTypeGrid[x, y];
                    if (fogType != FogType.Foggy) continue;
                    var position = new Vector3(x, y);
                    var fogObject = Globals.FogFactory.CreateFogObject(fogType, position, _levelWorldView.transform);
                    _fogObjectGrid[x, y] = fogObject;
                }
            }
        }

        public void Update()
        {
            _timer += Time.deltaTime;
        }

        public void TearDown()
        {
            TearDownFogObjectGrid();
            TearDownNodeObjectGrid();
            TearDownTerrainObjectGrid();
            Object.Destroy(_levelWorldView.gameObject);
            _levelWorldView = null;
        }

        private void TearDownFogObjectGrid()
        {
            if (_fogObjectGrid == null) return;

            int width = Data.Size.x;
            int height = Data.Size.y;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var fogObject = _fogObjectGrid[x, y];
                    if (fogObject == null) continue;
                    Object.Destroy(fogObject);
                    _fogObjectGrid[x, y] = null;
                }
            }

            _fogObjectGrid = null;
        }

        private void TearDownNodeObjectGrid()
        {
            if (_nodeObjectGrid == null) return;

            int width = Data.Size.x;
            int height = Data.Size.y;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var nodeObject = _nodeObjectGrid[x, y];
                    if (nodeObject == null) continue;
                    nodeObject.Dispose();
                    _nodeObjectGrid[x, y] = null;
                }
            }

            _nodeObjectGrid = null;
        }

        private void TearDownTerrainObjectGrid()
        {
            if (_terrainObjectGrid == null) return;

            int width = Data.Size.x;
            int height = Data.Size.y;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var terrainObject = _terrainObjectGrid[x, y];
                    if (terrainObject == null) continue;
                    Object.Destroy(terrainObject);
                    _terrainObjectGrid[x, y] = null;
                }
            }

            _terrainObjectGrid = null;
        }

        public Vector2Int ToLocation(Vector2 position)
        {
            return Vector2Int.RoundToInt(position);
        }

        public bool IsInBounds(Vector2Int location)
        {
            return (
                location.x >= 0 && location.x < Data.Size.x &&
                location.y >= 0 && location.y < Data.Size.y
            );
        }

        public bool HasNodeAt(Vector2Int location)
        {
            if (!IsInBounds(location))
                throw new LocationOutOfBoundsException($"{location}");

            return _nodeTypeGrid[location.x, location.y] != NodeType.Empty;
        }

        public NodeType GetNodeAt(Vector2Int location, out Node node) // TODO: Node를 리턴, Node안에 NodeType 접근 가능하게 변경
        {
            if (!IsInBounds(location))
                throw new LocationOutOfBoundsException($"{location}");

            node = _nodeObjectGrid[location.x, location.y];
            return _nodeTypeGrid[location.x, location.y];
        }

        public void SetNodeAt(Vector2Int location, NodeType newNodeType, out Node node)
        {
            node = null;

            if (!IsInBounds(location))
                throw new LocationOutOfBoundsException($"{location}");

            if (_nodeTypeGrid[location.x, location.y] != NodeType.Empty)
            {
                _availableLocations.Add(location);
                var nodeObject = _nodeObjectGrid[location.x, location.y];
                if (nodeObject != null)
                {
                    nodeObject.Dispose();
                    _nodeObjectGrid[location.x, location.y] = null;
                }
            }

            _nodeTypeGrid[location.x, location.y] = newNodeType;

            if (newNodeType != NodeType.Empty)
            {
                _availableLocations.Remove(location);
                var position = new Vector3(location.x, location.y);
                var nodeObject = Globals.NodeFactory.CreateNodeObject(newNodeType, position, _levelWorldView.transform);
                if (nodeObject != null) {
                    node = _nodeObjectGrid[location.x, location.y] = nodeObject.GetComponent<Node>();
                }
            }
        }

        public void SetLinkNodesAt(List<Vector2Int> locations)
        {
            GetNodeAt(locations[0], out Node startingNode);

            var isLinkable = true;
            for (int i = 1; i < locations.Count; i++)
            {
                var nodeLocation = locations[i];
                var hasNode = HasNodeAt(nodeLocation);
                if (hasNode) {
                    isLinkable = false;
                    break;
                }
            }

            if (!isLinkable) return;

            for (int i = 1; i < locations.Count; i++)
            {
                var nodeLocation = locations[i];
                var position = new Vector3(nodeLocation.x, nodeLocation.y);
                SetNodeAt(nodeLocation, NodeType.Belt, out Node node);
                var linkNode = node as LinkNode;
                if (linkNode == null) continue;
                linkNode.transportLines.Add(startingNode.transportLines[0]);
            }
        }

        public void UnlockCells(Vector2Int centerLocation, float radius)
        {
            var sqrRadius = radius * radius;
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.CeilToInt(radius); y++)
            {
                for (int x = -Mathf.FloorToInt(radius); x <= Mathf.CeilToInt(radius); x++)
                {
                    var sqrDistance = x * x + y * y;
                    if (sqrDistance > sqrRadius) continue;
                    var location = new Vector2Int(centerLocation.x + x, centerLocation.y + y);
                    UnlockCell(location);
                }
            }
        }

        private void UnlockCell(Vector2Int location)
        {
            if (!IsInBounds(location))
                throw new LocationOutOfBoundsException($"{location}");

            var fogObject = _fogObjectGrid[location.x, location.y];
            if (fogObject != null)
            {
                Object.Destroy(fogObject);
                _fogObjectGrid[location.x, location.y] = null;
            }

            GetNodeAt(location, out Node node);
            if (node != null)
                node.Show();
        }

        public void EarnMoney(int amount)
        {
            _earnings += amount;
        }

        public bool CheckObjectives()
        {
            return _earnings >= Data.ObjectiveEarnings;
        }
    }
}