using DG.Tweening;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class RegionNode : View
    {
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private int levelId;

        public bool IsLocked;

        public void Activate()
        {
            if (IsLocked) return;
            _bodyTransform.localPosition = new Vector3(_bodyTransform.localPosition.x, 0);
            _bodyTransform.DOLocalMoveY(_bodyTransform.localPosition.y + 0.1f, 0.1f).SetEase(Ease.OutQuad);
            _bodySpriteRenderer.DOColor(Color.white, 0.1f).SetEase(Ease.OutQuad);
        }

        public void Deactivate()
        {
            _bodyTransform.DOLocalMoveY(0f, 0.1f).SetEase(Ease.OutQuad);
            _bodySpriteRenderer.DOColor(Color.clear, 0.1f).SetEase(Ease.OutQuad);
        }

        public void Select()
        {
            if (IsLocked) return;

            _bodyTransform.localPosition = new Vector3(_bodyTransform.localPosition.x, 0);
            _bodySpriteRenderer.color = Color.clear;

            Debug.Log($"Region {levelId} selected.");
            Globals.MainSystem.GameManager.StartLevel(levelId);
        }
    }
}