using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View
{
    public class MainView : MonoBehaviour
    {
        [Header("UI")]
        public UI.Stage StageView;
        public UI.Laboratory LaboratoryView;

        [Header("World")]
        public World.Stage StageWorldView;
        public World.Laboratory LaboratoryWorldView;

        private void Awake()
        {
            Globals.MainView = this;
        }

        public void SetUp()
        {
            StageView.Show();
            LaboratoryView.Hide();

            StageWorldView.Show();
            LaboratoryWorldView.Hide();
        }
    }
}