using System;
using UnityEngine;

namespace SupplyChain.Data
{
    [Serializable]
    public class UpgradeFeature
    {
        public string Id;
        public float Value;
    }

    [CreateAssetMenu(fileName = "New Upgrade", menuName = "SupplyChain/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Cost;
        public UpgradeFeature[] Features;
    }
}