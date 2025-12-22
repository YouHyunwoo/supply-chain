using UnityEngine;

namespace SupplyChain.View.UI
{
    public class Upgrade : View
    {
        [SerializeField] private UpgradeInformation _upgradeInformationView;

        public void ShowInformation(string name, string description, int cost, Vector3 worldPosition)
        {
            _upgradeInformationView.SetName(name);
            _upgradeInformationView.SetDescription(description);
            _upgradeInformationView.SetCost(cost);
            _upgradeInformationView.Show();

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            RectTransform rectTransform = _upgradeInformationView.transform as RectTransform;
            var canvas = GetComponentInParent<Canvas>();
            var viewSize = rectTransform.sizeDelta * canvas.scaleFactor;
            var paddingSize = 10f;
            screenPosition.y += viewSize.y / 2 + paddingSize;

            rectTransform.position = screenPosition;

            _upgradeInformationView.ClampToScreen();
            _upgradeInformationView.AnimateOpen();
        }

        public void HideInformation()
        {
            _upgradeInformationView.Hide();
        }
    }
}