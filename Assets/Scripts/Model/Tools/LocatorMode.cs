using SupplyChain.View.World;
using UnityEngine;

namespace SupplyChain.Model
{
    public class LocatorMode : Tool
    {
        [SerializeField] private Node _processNodePrefab;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseScreenPosition = Input.mousePosition;
                var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                var mouseWorldLocation = Globals.MainSystem.GameManager.Game.CurrentLevel.ToLocation(mouseWorldPosition);
                // Globals.MainSystem.GameManager.Game.CurrentLevel.LocateNode(
                //     mouseWorldLocation,
                //     _processNodePrefab
                // );
                Globals.MainSystem.ToolManager.Select(Enum.ToolType.Transporter);
            }
        }
    }
}