using TMPro;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class Laboratory : View
    {
        public Upgrade UpgradeView;
        public RectTransform MapView;
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void SetMoney(int money) => _moneyText.text = $"$ {money}";
    }
}