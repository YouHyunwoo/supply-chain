using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _linksText;
    [SerializeField] private Button _addLinkButton;

    public void SetMoneyText(float amount)
    {
        _moneyText.text = $"$ {amount:0}";
    }

    public void SetLinkCountText(int linkCount)
    {
        _linksText.text = $"Links: {linkCount}";
    }
}