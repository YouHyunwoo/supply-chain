using UnityEngine;

namespace SupplyChain.Data
{
    [CreateAssetMenu(fileName = "New Level", menuName = "SupplyChain/Level")]
    public class Level : ScriptableObject
    {
        public Vector2Int Size;
        public int InitialSectorSize;
        public int ExtensiveSectorSize;
        public float Duration;
        public int ObjectiveEarnings;
    }
}