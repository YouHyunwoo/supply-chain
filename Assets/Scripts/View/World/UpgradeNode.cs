using DG.Tweening;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    [ExecuteAlways]
    public class UpgradeNode : MonoBehaviour
    {
        [SerializeField] private LineRenderer _edgePrefab;
        [SerializeField] private Data.Upgrade _upgradeData;
        public UpgradeNode[] ConnectedNodes;
        public bool IsUpgradable; // Debug

        private void Start()
        {
            UpdateLines();
        }

        public void UpdateLines()
        {
            // 기존 Edge들 제거 (역순으로 제거하여 누락 방지)
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child.name.StartsWith("Edge_"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            if (ConnectedNodes == null || ConnectedNodes.Length == 0 || _edgePrefab == null)
            {
                return;
            }

            for (int i = 0; i < ConnectedNodes.Length; i++)
            {
                if (ConnectedNodes[i] == null) continue;
                
                var edgeLineRenderer = Instantiate(_edgePrefab, transform);
                edgeLineRenderer.name = $"Edge_{i}_{ConnectedNodes[i].name}";
                edgeLineRenderer.useWorldSpace = true;
                edgeLineRenderer.positionCount = 2;
                edgeLineRenderer.SetPosition(0, transform.position);
                edgeLineRenderer.SetPosition(1, ConnectedNodes[i].transform.position);
            }
        }

        public void Activate()
        {
            transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad);

            if (_upgradeData == null) return;
            Globals.MainView.LaboratoryView.UpgradeView.ShowInformation(
                _upgradeData.Name,
                _upgradeData.Description,
                _upgradeData.Cost,
                transform.position + Vector3.up * 2f
            );
        }

        public void Deactivate()
        {
            transform.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
            Globals.MainView.LaboratoryView.UpgradeView.HideInformation();
        }

        public void Select()
        {
            if (IsUpgradable && Globals.MainSystem.Player.Money >= _upgradeData.Cost)
            {
                if (_upgradeData == null) return;

                Debug.Log("Upgrade applied successfully.");

                Globals.MainSystem.Player.Money -= _upgradeData.Cost;
                Globals.MainView.LaboratoryView.SetMoney(Globals.MainSystem.Player.Money);
                foreach (var feature in _upgradeData.Features)
                {
                    Globals.MainSystem.UpgradeManager.UpgradeFeature(feature);
                }
                // particle play
            }
            else
            {
                Debug.Log("Upgrade application failed.");

                transform.DOShakePosition(0.1f, 0.08f, 30, 90, false, false);
            }
        }
    }
}