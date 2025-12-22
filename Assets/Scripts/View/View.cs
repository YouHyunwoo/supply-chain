using UnityEngine;

namespace SupplyChain.View
{
    public class View : MonoBehaviour
    {
        public bool IsVisible => gameObject.activeSelf;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}