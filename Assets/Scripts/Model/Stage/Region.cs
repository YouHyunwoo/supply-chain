using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class Region : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private GameObject _fogPrefab;
        [SerializeField] private GameObject _sourceNodePrefab;
        [SerializeField] private GameObject _marketNodePrefab;
        [SerializeField] private GameObject _obstacleNodePrefab;
        [SerializeField] private GameObject _linkNodePrefab;
        [SerializeField] private float _cellSize = 1.0f;

        private Data.Region _regionData;

        GameObject[,] fogGrid;
        Node[,] nodeGrid;
        int nodeCount;
        private int _currentSectorIndex;
        private float _timer, _duration;
        private int _earnings;

        public int Width => (int)(_regionData.Size.x * _cellSize);
        public int Height => (int)(_regionData.Size.y * _cellSize);
        public Vector2 Center => new (_regionData.Size.x * _cellSize / 2, _regionData.Size.y * _cellSize / 2);
        public int SectorIndex => _currentSectorIndex;

        private void Update()
        {
            _timer += Time.deltaTime;
            Globals.MainView.StageView.Timer.SetTimeRatio((_duration - _timer) / _duration);
            if (_timer >= _duration)
            {
                Globals.MainSystem.StageManager.FailObjective();
            }
        }

        public void Create(Data.Region regionData)
        {
            _regionData = regionData;
            _timer = 0f;
            _duration = _regionData.Duration + Globals.MainSystem.Player.AdditionalTime;
            _earnings = 0;
            _currentSectorIndex = -1;

            CreateNodeGrid();
            CreateFogGrid();
            CreateNodes();
            UnlockNextSector();
        }

        private void CreateNodeGrid()
        {
            var regionSize = _regionData.Size;
            nodeGrid = new Node[regionSize.x, regionSize.y];
            nodeCount = 0;
        }

        private void CreateFogGrid()
        {
            var regionSize = _regionData.Size;
            fogGrid = new GameObject[regionSize.x, regionSize.y];

            for (int y = 0; y < regionSize.y; y++)
            {
                for (int x = 0; x < regionSize.x; x++)
                {
                    var position = new Vector3(x * _cellSize, y * _cellSize, 0);
                    Instantiate(_cellPrefab, position, Quaternion.identity, transform);

                    var fogObject = Instantiate(_fogPrefab, position, Quaternion.identity, transform);
                    fogGrid[x, y] = fogObject;
                }
            }
        }

        public void UnlockNextSector()
        {
            _currentSectorIndex++;
            UnlockSector(_currentSectorIndex);
        }

        public void UnlockSector(int sectorIndex)
        {
            var sectorSize = Mathf.Min(sectorIndex * _regionData.ExtensiveSectorSize + _regionData.InitialSectorSize + Globals.MainSystem.Player.AdditionalRegionSize, _regionData.Size.x);
            var centerLocation = new Vector2Int(_regionData.Size.x / 2, _regionData.Size.y / 2);

            for (int y = -sectorSize / 2; y <= sectorSize / 2; y++)
            {
                for (int x = -sectorSize / 2; x <= sectorSize / 2; x++)
                {
                    var location = new Vector2Int(centerLocation.x + x, centerLocation.y + y);
                    UnlockCell(location);
                }
            }
        }

        private void UnlockCell(Vector2Int location)
        {
            if (location.x < 0 || location.x >= _regionData.Size.x || location.y < 0 || location.y >= _regionData.Size.y)
            {
                Debug.LogError("Map: UnlockCell - Location out of bounds: " + location);
                return;
            }

            var fogObject = fogGrid[location.x, location.y];
            if (fogObject != null)
            {
                Destroy(fogObject);
                fogGrid[location.x, location.y] = null;
            }

            var node = GetNodeAt(location);
            if (node != null)
            {
                node.gameObject.SetActive(true);
            }
        }

        public void CreateNodes()
        {
            var location = new Vector2Int(_regionData.Size.x / 2 - 1, _regionData.Size.y / 2);
            var position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
            var sourceNodeObject = Instantiate(_sourceNodePrefab, position, Quaternion.identity, transform);
            SetNodeAt(location, sourceNodeObject.GetComponent<Node>());

            location = new Vector2Int(_regionData.Size.x / 2 + 1, _regionData.Size.y / 2);
            position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
            var marketNodeObject = Instantiate(_marketNodePrefab, position, Quaternion.identity, transform);
            SetNodeAt(location, marketNodeObject.GetComponent<Node>());

            var obstacleCount = _regionData.Size.x * _regionData.Size.y * 0.2f;
            for (int i = 0; i < obstacleCount && i < _regionData.Size.x * _regionData.Size.y; i++)
            {
                location = new Vector2Int(Random.Range(0, _regionData.Size.x), Random.Range(0, _regionData.Size.y));
                if (HasNodeAt(location))
                {
                    i--;
                    continue;
                }
                position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
                var obstacleNode = Instantiate(_obstacleNodePrefab, position, Quaternion.identity, transform);
                SetNodeAt(location, obstacleNode.GetComponent<Node>());
            }

            var veinCount = _regionData.Size.x * _regionData.Size.y * 0.1f;
            var centerLocation = new Vector2Int(_regionData.Size.x / 2, _regionData.Size.y / 2);
            for (int i = 0; i < veinCount && i < _regionData.Size.x * _regionData.Size.y; i++)
            {
                location = new Vector2Int(Random.Range(0, _regionData.Size.x), Random.Range(0, _regionData.Size.y));
                if (HasNodeAt(location) || Mathf.Abs(location.x - centerLocation.x) < 3 && Mathf.Abs(location.y - centerLocation.y) < 3)
                {
                    i--;
                    continue;
                }
                position = new Vector3(location.x * _cellSize, location.y * _cellSize, 0);
                sourceNodeObject = Instantiate(_sourceNodePrefab, position, Quaternion.identity, transform);
                sourceNodeObject.SetActive(false);
                SetNodeAt(location, sourceNodeObject.GetComponent<Node>());
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
            return location.x >= 0 && location.x < _regionData.Size.x && location.y >= 0 && location.y < _regionData.Size.y;
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

        public void AddEarnings(int amount)
        {
            _earnings += amount;

            if (_earnings >= _regionData.ObjectiveEarnings)
            {
                Debug.Log("[Region] 목표 수익 달성: " + _earnings + " / " + _regionData.ObjectiveEarnings);
                Globals.MainSystem.StageManager.AchieveObjective();
            }
        }
    }
}