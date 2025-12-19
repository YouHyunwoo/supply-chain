using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    // private static readonly SupplyChain.Model.Tool[] TOOLS =
    // {
    //     new SupplyChain.Model.Transporter(),
    //     new SupplyChain.Model.Tool(), // Inspector
    //     new SupplyChain.Model.Tool(), // PathCreator
    // };

    public List<SupplyChain.Data.Tool> ToolDataList;

    private ToolType _type;
    private bool[] _unlockedTools;
    // private SupplyChain.Model.Tool _currentTool;
    private ToolSlotItem _currentSelectedToolSlotItem;

    public ToolType Type => _type;
    public IReadOnlyList<bool> UnlockedTools => _unlockedTools;

    // private void Update()
    // {
    //     if (_currentTool != null)
    //     {
    //         _currentTool.Update();
    //     }
    // }

    public void SetUp()
    {
        _unlockedTools = new bool[ToolDataList.Count];
        for (int i = 0; i < ToolDataList.Count; i++)
        {
            _unlockedTools[i] = ToolDataList[i].IsUnlocked;
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
            var isBought = Globals.MainSystem.BuyTool(toolIndex, ToolDataList[toolIndex].UnlockCost);
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