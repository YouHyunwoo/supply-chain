using UnityEngine;

public class MarketNode : Node
{
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
            // Debug.Log("MarketNode: Selling ResourceCargo with Price " + resourceCargo.Price);
            Main.Instance.SellResourceCargo(resourceCargo.Price);
            resourceCargo.Dissolve();
        }
    }
}