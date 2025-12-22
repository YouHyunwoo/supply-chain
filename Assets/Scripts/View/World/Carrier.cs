using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Carrier : MonoBehaviour
    {
        private CircleCollider2D _collider;
        private readonly List<ResourceCargo> _carriedCargos = new ();

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
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
            if (_carriedCargos.Count >= Globals.MainSystem.Player.CarrierCapacity) return;

            // Debug.Log("[Player] Picking up ResourceCargo with Price " + resourceCargo.Price);
            // 카고의 생산지에 빠져나갔다고 알림
            resourceCargo.Follow(transform);
            if (_carriedCargos.Count > 0)
            {
                _carriedCargos[^1].Follow(resourceCargo.transform);
            }
            resourceCargo.Origin.ReleaseCargo();
            _carriedCargos.Add(resourceCargo);
        }

        public void UnfollowCargo(ResourceCargo cargo) // Cargo 운송 수간 = 플레이어 -> 다음 Cargo
        {
            var index = _carriedCargos.IndexOf(cargo);
            if (index == -1) return;

            cargo.Unfollow();
            _carriedCargos.RemoveAt(index);

            // 뒤에 있는 Cargo가 있으면 그 Cargo가 다음 Cargo를 따라가도록 설정
            if (index < _carriedCargos.Count)
            {
                if (index == 0)
                {
                    _carriedCargos[index].Follow(transform);
                }
                else
                {
                    _carriedCargos[index].Follow(_carriedCargos[index - 1].transform);
                }
            }
        }

        public void UnfollowAllCargo() // Cargo 운송 수간 = 플레이어 -> = null
        {
            foreach (var cargo in _carriedCargos)
            {
                cargo.Unfollow();
            }
            _carriedCargos.Clear();
        }

        public void SetInteractionRange(float range)
        {
            _collider.radius = range;
        }
    }
}