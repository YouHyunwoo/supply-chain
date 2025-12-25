using DG.Tweening;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class RegionNode : View
    {
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private int regionId;

        public bool IsLocked;

        public void Activate()
        {
            if (IsLocked) return;
            _bodyTransform.position = new Vector3(_bodyTransform.position.x, 0);
            _bodyTransform.DOMoveY(_bodyTransform.position.y + 0.15f, 0.1f).SetEase(Ease.OutQuad);
            _bodySpriteRenderer.DOColor(Color.white, 0.1f).SetEase(Ease.OutQuad);
        }

        public void Deactivate()
        {
            _bodyTransform.DOMoveY(0f, 0.1f).SetEase(Ease.OutQuad);
            _bodySpriteRenderer.DOColor(Color.clear, 0.1f).SetEase(Ease.OutQuad);
        }

        public void Select()
        {
            if (IsLocked) return;

            _bodyTransform.position = new Vector3(_bodyTransform.position.x, 0);
            _bodySpriteRenderer.color = Color.clear;

            Debug.Log($"Region {regionId} selected.");
            Globals.MainSystem.StageManager.StartStage(regionId);
        }
    }
}