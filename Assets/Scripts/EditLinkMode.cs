using System.Collections.Generic;
using UnityEngine;

public class EditLinkMode : MonoBehaviour
{
    private Vector2Int _lastLocation;
    private Node _startingNode;
    private readonly List<Vector2Int> _locations = new ();

    private void Update()
    {
        UpdateEditLinkMode();
    }
    
    private void UpdateEditLinkMode()
    {
        // UI가 아닌가?

        var mouse = Input.mousePosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, -Camera.main.transform.position.z));
        var worldLocation = Main.Instance.Map.ToLocation(new (worldPosition.x, worldPosition.y));

        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("EditLinkMode: Mouse Button Down at " + worldLocation);
            var isNodeAtLocation = Main.Instance.Map.HasNodeAt(worldLocation);
            if (!isNodeAtLocation) return;

            var startingNode = Main.Instance.Map.GetNodeAt(worldLocation);
            if (startingNode.OutputCount >= startingNode.MaxOutputCount)
            {
                // Debug.Log("EditLinkMode: Starting Node has reached max output count.");
                return;
            }
            _startingNode = startingNode;

            // Debug.Log("EditLinkMode: Starting Node at " + _startingNode);
            _locations.Clear();
            _locations.Add(worldLocation);
            _lastLocation = worldLocation;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_startingNode == null) return;

            // Debug.Log("EditLinkMode: Mouse Button Up at " + worldLocation);
            // var isNodeAtLocation = Main.Instance.Map.HasNodeAt(worldLocation);
            // if (isNodeAtLocation)
            {
                // var endingNode = Main.Instance.Map.GetNodeAt(worldLocation);
                // Debug.Log("EditLinkMode: Ending Node at " + endingNode);

                _startingNode.AddTransportLine(_locations);
                Main.Instance.Map.LocateLink(_locations);
            }

            _startingNode = null;
            _locations.Clear();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Debug.Log("EditLinkMode: Mouse Right Button Down - Canceling Link Edit Mode.");
            enabled = false;
            _startingNode = null;
            _locations.Clear();
            Main.Instance.SetMode(PlayerMode.Normal);
        }
        else
        {
            UpdateDrag(worldLocation);
        }

        _lastLocation = worldLocation;
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