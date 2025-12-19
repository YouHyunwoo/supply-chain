using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransporterMode : MonoBehaviour
{
    [SerializeField] private Collider2D _transferCollider;
    private List<ResourceCargo> _carriedCargos = new ();

    // Gemini 3 Flash: UI 위에 마우스가 있는지 확인, Physics2D Raycast가 콜라이더를 인식해서 무시하는 문제 해결
    public bool IsPointerOverUI()
    {
        // 현재 마우스 위치의 이벤트 데이터 생성
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // 레이캐스트 결과 리스트
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 결과 중에서 'UI 레이어'인 것만 있는지 확인
        foreach (RaycastResult result in results)
        {
            // 오브젝트의 레이어가 UI(5번 레이어)라면 UI 위에 있는 것으로 간주
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnEnable()
    {
        _transferCollider.enabled = false;
    }

    private void OnDisable()
    {
        UnfollowAllCargo();
        _transferCollider.enabled = false;
    }

    private void Update()
    {
        if (IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            _transferCollider.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log("[Player] Switching to Default Tool");
            UnfollowAllCargo();
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
        if (Globals.MainSystem.ToolManager.Type != ToolType.Transporter) return;

        var hasResourceCargo = collision.TryGetComponent<ResourceCargo>(out var resourceCargo);
        if (!hasResourceCargo) return;
        if (resourceCargo.IsBeingCarried) return;

        // Debug.Log("[Player] Picking up ResourceCargo with Price " + resourceCargo.Price);
        resourceCargo.Follow(transform);
        _carriedCargos.Add(resourceCargo);
    }

    private void UnfollowAllCargo() // Cargo 운송 수간 = 플레이어 -> = null
    {
        foreach (var cargo in _carriedCargos)
        {
            cargo.Unfollow();
        }
        _carriedCargos.Clear();
    }
}