using UnityEngine;
using UnityEngine.UI;

namespace SupplyChain.View.UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Image _barImage;

        public void SetTimeRatio(float ratio)
        {
            _barImage.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}