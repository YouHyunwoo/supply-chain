using SupplyChain.Model;
using UnityEngine;

namespace SupplyChain.View.World
{
    public class ProcessNode : Node
    {
        [SerializeField] private ResourceCargo _resourceCargoPrefab;
        [SerializeField] private float _processSpeed = 0.1f;

        private float _timer = 0.0f;
        private SourceNode _originNode = null;
        private float _value = 0;
        private float _amount = 0;
        private ResourceCargo _inputCargo;

        private void Update()
        {
            UpdateResourceGeneration();
        }

        private void UpdateResourceGeneration()
        {
            if (_inputCargo == null) return;
            Debug.Log("Processing ResourceCargo...");

            _timer += Time.deltaTime * _processSpeed * 1f;
            SetProgress(_timer);
            if (_timer >= 1.0f)
            {
                _timer -= 1.0f;
                GenerateResourceCargo();
            }
        }

        private void GenerateResourceCargo()
        {
            if (_currentCargo == null)
            {
                _currentCargo = Instantiate(_resourceCargoPrefab, transform.position, Quaternion.identity);
                _currentCargo.Origin = this;
                Globals.SoundManager.PlaySFX(Globals.Database.SFXClips[1]);
            }

            _currentCargo.Value = _value;
            var actualAmount = _currentCargo.Amount + _amount;
            _currentCargo.Amount = actualAmount;

            if (transportLines.Count > 0)
            {
                var transportLine = transportLines[0];
                var transport = new Transport
                {
                    cargo = _currentCargo,
                };
                transportLine.transports.Add(transport);
                _currentCargo.StartTransport();
                ReleaseCargo();
            }

            _inputCargo.Dissolve();
            _inputCargo = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hasCargo = collision.TryGetComponent<Cargo>(out var cargo);
            if (!hasCargo) return;

            ReceiveCargo(cargo);
        }

        public override void ReceiveCargo(Cargo cargo)
        {
            var resourceCargo = cargo as ResourceCargo;
            if (resourceCargo != null)
            {
                _inputCargo = resourceCargo;
                _value = resourceCargo.Value;
                _amount = resourceCargo.Amount;
                // Debug.Log("MarketNode: Selling ResourceCargo with Price " + resourceCargo.Price);
                // Globals.MainSystem.SellResourceCargo(resourceCargo.Price);
                // resourceCargo.Dissolve();
                resourceCargo.gameObject.SetActive(false);
            }
        }
    }
}