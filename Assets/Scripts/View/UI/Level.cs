using TMPro;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class Level : View
    {
        public ToolSlotList ToolSlotList;
        public Timer Timer;
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void SetMoney(int amount) => _moneyText.text = $"$ {amount}";
    }
}