using TMPro;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class Stage : View
    {
        public ToolSlotList ToolSlotList;
        public Timer Timer;
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void SetMoneyText(float amount) => _moneyText.text = $"$ {amount:0}";
    }
}