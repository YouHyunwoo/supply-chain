using UnityEngine;
using UnityEngine.UI;

namespace SupplyChain.View.UI
{
    public class FadeOverlay : TransitionOverlay
    {
        [SerializeField] private Image _fadeImage;

        protected override void HandleTransition(float transitionValue, float startValue, float endValue)
        {
            Color color = _fadeImage.color;
            color.a = transitionValue;
            _fadeImage.color = color;
        }
    }
}