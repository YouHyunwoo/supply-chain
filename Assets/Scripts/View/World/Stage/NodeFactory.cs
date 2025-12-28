using SupplyChain.Enum;
using SupplyChain.Exception;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class NodeFactory : View
    {
        [SerializeField] private GameObject _sourceNodePrefab;
        [SerializeField] private GameObject _marketNodePrefab;
        [SerializeField] private GameObject _obstacleNodePrefab;
        [SerializeField] private GameObject _linkNodePrefab;
        [SerializeField] private GameObject _processNodePrefab;

        private void Awake()
        {
            Globals.NodeFactory = this;
        }

        public GameObject CreateNodeObject(NodeType nodeType, Vector3 position, Transform parentTransform)
        {
            return nodeType switch
            {
                NodeType.Source => Instantiate(_sourceNodePrefab, position, Quaternion.identity, parentTransform),
                NodeType.Market => Instantiate(_marketNodePrefab, position, Quaternion.identity, parentTransform),
                NodeType.Process => Instantiate(_processNodePrefab, position, Quaternion.identity, parentTransform),
                NodeType.Belt => Instantiate(_linkNodePrefab, position, Quaternion.identity, parentTransform),
                _ => throw new UnknownNodeTypeException($"{nodeType}"),
            };
        }
    }
}