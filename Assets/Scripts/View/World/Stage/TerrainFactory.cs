using SupplyChain.Enum;
using SupplyChain.Exception;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class TerrainFactory : View
    {
        [SerializeField] private GameObject _emptyCellPrefab;
        [SerializeField] private GameObject _blockCellPrefab;

        private void Awake()
        {
            Globals.TerrainFactory = this;
        }

        public GameObject CreateTerrainObject(TerrainType terrainType, Vector3 position, Transform parentTransform)
        {
            return terrainType switch
            {
                TerrainType.Empty => Instantiate(_emptyCellPrefab, position, Quaternion.identity, parentTransform),
                TerrainType.Block => Instantiate(_blockCellPrefab, position, Quaternion.identity, parentTransform),
                _ => throw new UnknownTerrainTypeException($"{terrainType}"),
            };
        }
    }
}