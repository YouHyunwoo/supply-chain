using System.Collections.Generic;
using SupplyChain.Model;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathCreatorMode : MonoBehaviour
{
    private Vector2Int _lastLocation;
    private Node _startingNode;
    private readonly List<Vector2Int> _locations = new ();

    private void Update()
    {
        UpdateEditLinkMode();
    }

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

    private void UpdateEditLinkMode()
    {
        // if (IsPointerOverUI()) return;

        // var mouse = Input.mousePosition;
        // var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, -Camera.main.transform.position.z));
        // var worldLocation = Globals.MainSystem.Map.ToLocation(new (worldPosition.x, worldPosition.y));

        // if (Input.GetMouseButtonDown(0))
        // {
        //     // Debug.Log("EditLinkMode: Mouse Button Down at " + worldLocation);
        //     var isNodeAtLocation = Globals.MainSystem.Map.HasNodeAt(worldLocation);
        //     if (!isNodeAtLocation) return;

        //     var startingNode = Globals.MainSystem.Map.GetNodeAt(worldLocation);
        //     if (startingNode.OutputCount >= startingNode.MaxOutputCount)
        //     {
        //         // Debug.Log("EditLinkMode: Starting Node has reached max output count.");
        //         return;
        //     }
        //     _startingNode = startingNode;

        //     // Debug.Log("EditLinkMode: Starting Node at " + _startingNode);
        //     _locations.Clear();
        //     _locations.Add(worldLocation);
        //     _lastLocation = worldLocation;
        // }
        // else if (Input.GetMouseButtonUp(0))
        // {
        //     if (_startingNode == null) return;

        //     // Debug.Log("EditLinkMode: Mouse Button Up at " + worldLocation);
        //     // var isNodeAtLocation = Main.Instance.Map.HasNodeAt(worldLocation);
        //     // if (isNodeAtLocation)
        //     {
        //         // var endingNode = Main.Instance.Map.GetNodeAt(worldLocation);
        //         // Debug.Log("EditLinkMode: Ending Node at " + endingNode);

        //         _startingNode.AddTransportLine(_locations);
        //         Globals.MainSystem.Map.LocateLink(_locations);
        //     }

        //     _startingNode = null;
        //     _locations.Clear();
        // }
        // else if (Input.GetMouseButtonDown(1))
        // {
        //     // Debug.Log("EditLinkMode: Mouse Right Button Down - Canceling Link Edit Mode.");
        //     enabled = false;
        //     _startingNode = null;
        //     _locations.Clear();
        //     Globals.MainSystem.Tool.Select(ToolType.Transporter);
        // }
        // else
        // {
        //     UpdateDrag(worldLocation);
        // }

        // _lastLocation = worldLocation;
    }

    private void UpdateDrag(Vector2Int worldLocation)
    {
        if (_startingNode == null) return;
        if (worldLocation == _lastLocation) return;

        var toward = worldLocation - _lastLocation;
        if (Mathf.Abs(toward.x) + Mathf.Abs(toward.y) != 1) return;

        if (_locations.Count > 1 && worldLocation == _locations[^2])
        {
            // 한 칸 되돌리기
            _locations.RemoveAt(_locations.Count - 1);
        }
        else
        {
            // 한 칸 진행하기
            _locations.Add(worldLocation);
        }

        _startingNode.UpdateTransportLineCandidate(_locations);
    }
}