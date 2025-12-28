using UnityEngine;

namespace SupplyChain.Data
{
    [CreateAssetMenu(fileName = "New Game", menuName = "SupplyChain/Game")]
    public class Game : ScriptableObject
    {
        public Level[] Levels;
    }
}