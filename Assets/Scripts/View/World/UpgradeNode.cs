using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.View.World
{
    [ExecuteAlways]
    public class UpgradeNode : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private LineRenderer _edgePrefab;
        public UpgradeNode[] ConnectedNodes;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.2f, 0.2f).SetEase(Ease.InBack);
            // 설명 보이기
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
            // 설명 숨기기
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Upgrade Node Clicked");
        }
    }
}