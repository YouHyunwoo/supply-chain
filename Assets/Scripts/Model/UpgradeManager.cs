using SupplyChain.Data;
using UnityEngine;

namespace SupplyChain.Model
{
    public class UpgradeManager : MonoBehaviour
    {
        public void UpgradeFeature(UpgradeFeature feature)
        {
            Debug.Log($"Upgrading with ID: {feature.Id}");

            switch (feature.Id)
            {
                case "increase-earnings":
                    Globals.MainSystem.Player.EarningsMultiplier += feature.Value;
                    break;
                case "lightweight-1":
                case "lightweight-2":
                case "lightweight-3":
                    Globals.MainSystem.Player.CarrierSpeedMultiplier = feature.Value;
                    break;
                case "capacity-1":
                case "capacity-2":
                case "capacity-3":
                    Globals.MainSystem.Player.CarrierCapacity += (int)feature.Value;
                    break;
                case "mining-speed-1":
                case "mining-speed-2":
                case "mining-speed-3":
                    Globals.MainSystem.Player.SourceGenerationSpeedMultiplier = feature.Value;
                    break;
            }
        }
    }
}