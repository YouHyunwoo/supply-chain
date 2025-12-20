using UnityEngine;

namespace SupplyChain.Model
{
    public class Tool : MonoBehaviour
    {
        public void Select()
        {
            OnSelected();
        }

        protected virtual void OnSelected()
        {
            
        }

        public void Deselect()
        {
            OnDeselected();
        }

        protected virtual void OnDeselected()
        {
            
        }
    }
}