using Cysharp.Threading.Tasks;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View
{
    public class MainView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private View _lockOverlay;
        [SerializeField] private UI.FadeOverlay _fadeOverlay;
        public UI.Level LevelView;
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
            LevelView.Show();
            LaboratoryView.Hide();

            StageWorldView.Show();
            LaboratoryWorldView.Hide();
        }

        public void LockScreen()
            => _lockOverlay.Show();
        public void UnlockScreen()
            => _lockOverlay.Hide();

        public async UniTask FadeOut()
            => await _fadeOverlay.StartTransitionIn();
        public async UniTask FadeIn()
            => await _fadeOverlay.StartTransitionOut();
    }
}