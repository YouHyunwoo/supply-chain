using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class UpgradeInformation : View
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private ScreenClamper _screenClamper;

        private Tween tween;

        public void SetName(string name) => _nameText.text = name;
        public void SetDescription(string description) => _descriptionText.text = description;
        public void SetCost(int cost) => _costText.text = $"$ {cost}";
        public void ClampToScreen() { if (_screenClamper != null) _screenClamper.ClampToScreen(); }
        public void AnimateOpen()
        {
            if (tween != null && tween.IsActive()) tween.Kill();
            transform.localScale = Vector3.one * 0.5f;
            tween = DOTween.Sequence()
                .Append(transform.DOScale(1f, 0.1f).SetEase(Ease.OutBack));
        }
    }
}