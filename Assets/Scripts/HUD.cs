using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI linksText;
    [SerializeField] private Button _addLinkButton;

    public void SetMoneyText(float amount)
    {
        moneyText.text = $"$ {amount:0}";
    }

    public void SetLinksText(int linkCount)
    {
        linksText.text = $"Links: {linkCount}";
    }
}