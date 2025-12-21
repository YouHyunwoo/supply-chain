using TMPro;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class UpgradeInformation : View
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;

        public void SetName(string name) => _nameText.text = name;
        public void SetDescription(string description) => _descriptionText.text = description;
        public void SetCost(int cost) => _costText.text = $"$ {cost}";
    }
}