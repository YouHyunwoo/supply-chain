using SupplyChain.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.View
{
    public class Fog : MonoBehaviour
    {
        public void OnPointerClick(BaseEventData eventData)
        {
            if (eventData is PointerEventData pointerData && pointerData.button == PointerEventData.InputButton.Left)
            {
                // Globals.MainSystem.Map.UnlockNextSector();
            }
        }
    }
}