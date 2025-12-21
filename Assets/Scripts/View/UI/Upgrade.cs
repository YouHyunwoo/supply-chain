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

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            // Debug.Log("월드 위치: " + worldPosition + " 스크린 위치: " + screenPosition);

            RectTransform rectTransform = _upgradeInformationView.transform as RectTransform;
            var canvas = _upgradeInformationView.GetComponentInParent<Canvas>();
            var parentRect = rectTransform.parent as RectTransform;
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, 
                (Vector2)screenPosition, 
                camera, 
                out Vector2 localPoint
            );

            // Debug.Log(localPoint);

            var parentMin = parentRect.rect.min;
            var parentMax = parentRect.rect.max;
            var childSize = rectTransform.rect.size;
            var pivot = rectTransform.pivot;

            var min = parentMin + childSize * pivot;
            var max = parentMax - childSize * (Vector2.one - pivot);
            // Debug.Log("Parent Min: " + parentMin + ", Parent Max: " + parentMax);
            // Debug.Log("Child Size: " + childSize + ", Pivot: " + pivot);
            // Debug.Log("Min: " + min + ", Max: " + max);

            // float minX = size.x * pivot.x;
            // float maxX = canvasWidth - (size.x * (1f - pivot.x));
            // float minY = size.y * pivot.y;
            // float maxY = canvasHeight - (size.y * (1f - pivot.y));
            // Debug.Log(minX + ", " + maxX + ", " + minY + ", " + maxY);
            // Debug.Log(Screen.width + ", " + Screen.height);

            localPoint.x = Mathf.Clamp(screenPosition.x, min.x, max.x);
            localPoint.y = Mathf.Clamp(screenPosition.y, min.y, max.y);
            // Debug.Log(localPoint);

            rectTransform.position = screenPosition;

            _upgradeInformationView.Show();
        }

        public void HideInformation()
        {
            _upgradeInformationView.Hide();
        }
    }
}