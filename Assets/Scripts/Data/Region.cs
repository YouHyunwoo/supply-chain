using UnityEngine;

namespace SupplyChain.Data
{
    [CreateAssetMenu(fileName = "New Region", menuName = "SupplyChain/Region")]
    public class Region : ScriptableObject
    {
        public Vector2Int Size;
        public int InitialSectorSize;
        public int ExtensiveSectorSize;
        public float Duration;
        public int ObjectiveEarnings;
    }
}