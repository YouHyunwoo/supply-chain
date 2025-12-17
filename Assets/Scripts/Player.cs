using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Collider2D _transferCollider;
    [SerializeField] private TransferCargoMode transferCargoMode;
    [SerializeField] private EditLinkMode editLinkMode;

    private void Update()
    {
        if (Main.Instance.Mode != PlayerMode.Normal) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            // Debug.Log("Player: Switching to EditLink mode");
            editLinkMode.enabled = true;
            Main.Instance.SetMode(PlayerMode.EditLink);
        }

        if (Main.Instance.Mode != PlayerMode.Normal) return;
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("Player: Switching to TransferCargo mode");
            transferCargoMode.enabled = true;
            _transferCollider.enabled = true;
            Main.Instance.SetMode(PlayerMode.TransferCargo);
        }
    }
}