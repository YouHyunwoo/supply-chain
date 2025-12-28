using System;
using SupplyChain.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolSlotItem : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private GameObject _lockedOverlay;
    [SerializeField] private TextMeshProUGUI _lockedText;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Button _overlayButton;

    private ToolType _toolType;
    private bool _isSelected = false;
    private int _number;

    public ToolType ToolType => _toolType;
    public int ToolNumber => _number;
    public bool IsSelected => _isSelected;

    public event Action<GameObject> OnClicked;

    private void Awake()
    {
        _overlayButton.onClick.AddListener(HandleClicked);
    }

    private void HandleClicked() => OnClicked?.Invoke(gameObject);

    public void SetType(ToolType type) => _toolType = type;
    public void SetIcon(Sprite icon) => _iconImage.sprite = icon;
    public void SetCostText(string text) => _costText.text = text;
    public void SetNumber(int number)
    {
        _number = number;
        _numberText.text = number.ToString();
    }
    public void SetLocked(bool locked) => _lockedOverlay.SetActive(locked);
    public void SetLockedText(string text) => _lockedText.text = text;
    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        _frameImage.color = selected ? Color.yellow : Color.white;
    }
}