using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.View.World
{
    public class RegionNodeCollider : View,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private RegionNode _regionNode;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _regionNode.Activate();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _regionNode.Deactivate();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _regionNode.Select();
        }
    }
}