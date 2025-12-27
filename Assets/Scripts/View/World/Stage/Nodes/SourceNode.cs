using SupplyChain.Model;
using UnityEngine;

public class SourceNode : Node
{
    [SerializeField] private ResourceCargo _resourceCargoPrefab;
    [SerializeField] private float _generationSpeed = 0.3f;
    [SerializeField] private float _capacity = 1.0f;
    [SerializeField] private float _resourceValue = 1.0f;
    [SerializeField] private float _resourceAmount = 1.0f;

    private float _timer = 0.0f;

    private void Update()
    {
        UpdateResourceGeneration();
    }

    private void UpdateResourceGeneration()
    {
        if (_currentCargo != null) return;

        _timer += Time.deltaTime * _generationSpeed * Globals.MainSystem.Player.SourceGenerationSpeedMultiplier;
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

        _currentCargo.Value = _resourceValue;
        var actualAmount = Mathf.Min(_currentCargo.Amount + _resourceAmount, _capacity);
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
    }
}