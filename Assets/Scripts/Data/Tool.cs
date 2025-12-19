using UnityEngine;

namespace SupplyChain.Data
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "SupplyChain/Tool")]
    public class Tool : ScriptableObject
    {
        public ToolType Type;
        public bool IsUnlocked;
        public int UnlockCost;
        public int UseCost;
        public Sprite Icon;
    }
}