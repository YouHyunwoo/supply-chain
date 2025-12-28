using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class TransitionOverlay : View
    {
        public float FadeDuration = 1.0f;
        protected float startValue;
        protected float endValue;
        protected float transitionValue; // 0: visible, 1: transitioned
        private bool _isTransitioning;
        private float _direction;
        public bool IsFading => enabled;

        private async UniTask UpdateTransitioning()
        {
            _isTransitioning = true;

            while (true)
            {
                transitionValue += _direction * Time.deltaTime / FadeDuration;
                transitionValue = Mathf.Clamp01(transitionValue);
                HandleTransition(transitionValue, startValue, endValue);
                await UniTask.Yield();
                if ((_direction > 0 && transitionValue >= endValue) ||
                    (_direction < 0 && transitionValue <= endValue))
                {
                    break;
                }
            }

            HandleTransitionFinished();
            _isTransitioning = false;
        }

        public async UniTask StartTransitionIn()
        {
            Show();
            await StartTransition(0, 1);
        }

        public async UniTask StartTransitionOut()
        {
            await StartTransition(1, 0);
            Hide();
        }

        private async UniTask StartTransition(float startValue, float endValue)
        {
            if (startValue == endValue)
            {
                Debug.LogWarning("[FadeOverlay] 잘못된 페이드 값 설정입니다. 시작 값이 종료 값보다 크거나 같습니다.");
                return;
            }
            this.startValue = startValue;
            this.endValue = endValue;
            transitionValue = startValue;
            _direction = Mathf.Sign(endValue - startValue);
            HandleTransitionStarted(transitionValue, startValue, endValue);
            if (!_isTransitioning)
                await UpdateTransitioning();
        }

        protected virtual void HandleTransitionStarted(float transitionValue, float startValue, float endValue) { }

        protected virtual void HandleTransition(float transitionValue, float startValue, float endValue) { }

        protected virtual void HandleTransitionFinished() { }
    }
}