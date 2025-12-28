using Mono.Cecil;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class ResourceCargo : Cargo
    {
        public Resource ResourceType;
        public bool isTransported = false;
        private Transform _carrier;

        public int Price => (int)(Value * Amount);
        public bool IsBeingCarried => _carrier != null;

        public void Dissolve()
        {
            if (_carrier != null && _carrier.TryGetComponent<Carrier>(out var carrier))
            {
                carrier.UnfollowCargo(this);
            }
            _carrier = null;
            Origin = null;
            Destroy(gameObject);
        }

        public void Follow(Transform target) => _carrier = target;

        public void Unfollow() => _carrier = null;

        public override void StartTransport() => isTransported = true;

        public override void CompleteTransport() => isTransported = false;

        void Update()
        {
            if (isTransported) return;
            if (_carrier != null)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    _carrier.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0),
                    Time.deltaTime * 10.0f * Globals.MainSystem.Player.CarrierSpeedMultiplier
                );
            }
        }
    }
}