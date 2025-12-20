using System.Collections.Generic;
using UnityEngine;

namespace SupplyChain.Model
{
    public class ToolManager : MonoBehaviour
    {
        private ToolType _type;
        private bool[] _unlockedTools;
        private ToolSlotItem _currentSelectedToolSlotItem;

        public ToolType Type => _type;
        public IReadOnlyList<bool> UnlockedTools => _unlockedTools;

        public void SetUp()
        {
            var ToolDataArray = Globals.Database.Tools;
            _unlockedTools = new bool[ToolDataArray.Length];
            for (int i = 0; i < ToolDataArray.Length; i++)
            {
                _unlockedTools[i] = ToolDataArray[i].IsUnlocked;
            }
        }

        public void Select(ToolType type)
        {
            foreach (var toolSlotItem in Globals.MainView.StageView.ToolSlotList.ToolSlotItems)
            {
                if (toolSlotItem.ToolType == type)
                {
                    HandleToolSelected(toolSlotItem.gameObject);
                    return;
                }
            }
        }

        public void Unlock(int toolIndex)
        {
            if (toolIndex < 0 || toolIndex >= _unlockedTools.Length)
            {
                Debug.LogError("[Tool] Invalid tool index: " + toolIndex);
                return;
            }

            _unlockedTools[toolIndex] = true;
        }

        public void HandleToolSelected(GameObject toolSlotItemObject)
        {
            var toolSlotItem = toolSlotItemObject.GetComponent<ToolSlotItem>();
            var toolSlotItemNumber = toolSlotItem.ToolNumber;
            var toolIndex = toolSlotItemNumber - 1;

            if (!_unlockedTools[toolIndex])
            {
                var isBought = Globals.MainSystem.BuyTool(toolIndex, Globals.Database.Tools[toolIndex].UnlockCost);
                if (isBought)
                {
                    toolSlotItem.SetLocked(false);
                }
                else return;
            }

            if (_currentSelectedToolSlotItem != null)
                _currentSelectedToolSlotItem.SetSelected(false);
            _currentSelectedToolSlotItem = toolSlotItem;
            if (_currentSelectedToolSlotItem != null)
                _currentSelectedToolSlotItem.SetSelected(true);

            _type = _currentSelectedToolSlotItem.ToolType;
            Globals.MainSystem.Player.SetMode(_type);
        }
    }
}