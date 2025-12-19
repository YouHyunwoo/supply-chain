using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class ToolSlotList : MonoBehaviour
    {
        [SerializeField] private RectTransform _toolSlotItemContainer;
        [SerializeField] private ToolSlotItem _toolSlotItemPrefab;

        private readonly List<ToolSlotItem> _toolSlotItems = new ();

        public IReadOnlyList<ToolSlotItem> ToolSlotItems => _toolSlotItems.AsReadOnly();

        public void SetUpToolSlots(List<Data.Tool> toolDataList, Action<GameObject> onClicked, int defaultSelectedIndex = 0)
        {
            foreach (var toolSlotItem in _toolSlotItems)
            {
                Destroy(toolSlotItem.gameObject);
            }
            _toolSlotItems.Clear();

            for (int i = 0; i < toolDataList.Count; i++)
            {
                var toolData = toolDataList[i];
                var toolSlotItemObject = Instantiate(_toolSlotItemPrefab, _toolSlotItemContainer);
                var toolSlotItem = toolSlotItemObject.GetComponent<ToolSlotItem>();
                toolSlotItem.SetType(toolData.Type);
                toolSlotItem.SetNumber(i + 1);
                toolSlotItem.SetIcon(toolData.Icon);
                toolSlotItem.SetCostText($"$ {toolData.UseCost}");
                toolSlotItem.SetLocked(!toolData.IsUnlocked);
                toolSlotItem.SetLockedText($"$ {toolData.UnlockCost}");
                toolSlotItem.OnClicked += onClicked;
                _toolSlotItems.Add(toolSlotItem);
            }

            _toolSlotItems[defaultSelectedIndex].SetSelected(true);
        }
    }
}