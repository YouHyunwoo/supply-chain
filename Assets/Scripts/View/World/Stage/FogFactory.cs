using SupplyChain.Enum;
using SupplyChain.Exception;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class FogFactory : View
    {
        [SerializeField] private GameObject _fogObjectPrefab;

        private void Awake()
        {
            Globals.FogFactory = this;
        }

        public GameObject CreateFogObject(FogType fogType, Vector3 position, Transform parentTransform)
        {
            return fogType switch
            {
                FogType.Cleared => null,
                FogType.Foggy => Instantiate(_fogObjectPrefab, position, Quaternion.identity, parentTransform),
                _ => throw new UnknownFogTypeException($"{fogType}"),
            };
        }
    }
}