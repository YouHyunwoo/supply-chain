using System.Collections.Generic;
using UnityEngine;

public class TransferCargoMode : MonoBehaviour
{
    [SerializeField] private Collider2D _transferCollider;
    private List<ResourceCargo> _carriedCargos = new ();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log("Player: Switching to Normal mode");
            UnfollowAllCargo();
            Main.Instance.SetMode(PlayerMode.Normal);
            enabled = false;
            _transferCollider.enabled = false;
        }
        else
        {
            var mouse = Input.mousePosition;
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, -Camera.main.transform.position.z));
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Main.Instance.Mode != PlayerMode.TransferCargo) return;

        var hasResourceCargo = collision.TryGetComponent<ResourceCargo>(out var resourceCargo);
        if (!hasResourceCargo) return;
        if (resourceCargo.IsBeingCarried) return;

        // Debug.Log("Player: Picking up ResourceCargo with Price " + resourceCargo.Price);
        resourceCargo.Follow(transform);
        _carriedCargos.Add(resourceCargo);
    }

    private void UnfollowAllCargo()
    {
        foreach (var cargo in _carriedCargos)
        {
            cargo.Unfollow();
        }
        _carriedCargos.Clear();
    }
}