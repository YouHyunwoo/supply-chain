using System;
using UnityEngine;

namespace SupplyChain.Data
{
    [Serializable]
    public class UpgradeEffect
    {
        public string Name;
        public float Value;
    }

    [CreateAssetMenu(fileName = "New Upgrade", menuName = "SupplyChain/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Cost;
        public UpgradeEffect[] Effects;
    }
}