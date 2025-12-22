using UnityEngine;

namespace SupplyChain.View.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ScreenClamper : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        public void ClampToScreen()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            var viewSize = _rectTransform.sizeDelta * _canvas.scaleFactor;
            var min = Vector2.Scale(viewSize, _rectTransform.pivot);
            var max = screenSize - Vector2.Scale(viewSize, Vector2.one - _rectTransform.pivot);
            var x = Mathf.Clamp(_rectTransform.position.x, min.x, max.x);
            var y = Mathf.Clamp(_rectTransform.position.y, min.y, max.y);
            _rectTransform.position = new Vector2(x, y);
        }
    }
}