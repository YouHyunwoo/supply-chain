using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    [RequireComponent(typeof(Collider2D))]
    public class Carrier : MonoBehaviour
    {
        private Collider2D _collider;
        private readonly List<ResourceCargo> _carriedCargos = new ();

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _collider.enabled = true;
        }

        private void OnDisable()
        {
            UnfollowAllCargo();
            _collider.enabled = false;
        }

        private void Update()
        {
            var mouse = Input.mousePosition;
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, -Camera.main.transform.position.z));
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Globals.MainSystem.ToolManager.Type != ToolType.Transporter) return;

            var hasResourceCargo = collision.TryGetComponent<ResourceCargo>(out var resourceCargo);
            if (!hasResourceCargo) return;
            if (resourceCargo.IsBeingCarried) return;

            // Debug.Log("[Player] Picking up ResourceCargo with Price " + resourceCargo.Price);
            // 카고의 생산지에 빠져나갔다고 알림
            resourceCargo.Follow(transform);
            _carriedCargos.Add(resourceCargo);
        }

        public void UnfollowAllCargo() // Cargo 운송 수간 = 플레이어 -> = null
        {
            foreach (var cargo in _carriedCargos)
            {
                cargo.Unfollow();
            }
            _carriedCargos.Clear();
        }
    }
}