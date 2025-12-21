using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.View.World
{
    public class UpgradeNodeCollider : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private UpgradeNode _upgradeNode;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _upgradeNode.Activate();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _upgradeNode.Deactivate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _upgradeNode.Select();
        }
    }
}