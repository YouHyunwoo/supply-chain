using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.UI
{
    public class MainView : MonoBehaviour
    {
        public Stage StageView;
        public Laboratory LaboratoryView;

        private void Awake()
        {
            Globals.MainView = this;
        }

        public void SetUp()
        {
            
        }
    }
}