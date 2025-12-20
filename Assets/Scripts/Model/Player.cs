using UnityEngine;

namespace SupplyChain.Model
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transporter _transporterMode;
        // [SerializeField] private InspectorMode _inspectorMode;
        [SerializeField] private PathCreatorMode _pathCreatorMode;

        public int Money;
        public float AdditionalTime;
        public float EarningsMultiplier;
        public float SourceGenerationSpeedMultiplier;
        public int AdditionalRegionSize;

        public void SetMode(ToolType toolType)
        {
            _transporterMode.enabled = toolType == ToolType.Transporter;
            // _inspectorMode.enabled = toolType == ToolType.Inspector;
            _pathCreatorMode.enabled = toolType == ToolType.PathCreator;
        }
    }
}