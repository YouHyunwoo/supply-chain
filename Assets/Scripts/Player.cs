using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TransporterMode _transporterMode;
    // [SerializeField] private InspectorMode _inspectorMode;
    [SerializeField] private PathCreatorMode _pathCreatorMode;

    public int Money;

    public void SetMode(ToolType toolType)
    {
        _transporterMode.enabled = toolType == ToolType.Transporter;
        // _inspectorMode.enabled = toolType == ToolType.Inspector;
        _pathCreatorMode.enabled = toolType == ToolType.PathCreator;
    }
}