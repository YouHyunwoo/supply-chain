using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.View.World
{
    public class Upgrade : View
    {
        private Camera _mainCamera;
        private bool _isDragging;
        private Vector2 _dragStartPosition;
        private Vector3 _cameraStartPosition;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private bool IsPointerOverUI()
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

        private void Update()
        {
            if (IsPointerOverUI()) return;

            if (Input.GetMouseButtonDown(0))
            {
                _dragStartPosition = Input.mousePosition;
                _cameraStartPosition = _mainCamera.transform.position;
                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
            else
            {
                if (_isDragging)
                {
                    var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var dragStartWorldPosition = _mainCamera.ScreenToWorldPoint(_dragStartPosition);
                    var dragDelta = mouseWorldPosition - dragStartWorldPosition;

                    var newCameraPosition = _cameraStartPosition - new Vector3(dragDelta.x, dragDelta.y, 0);
                    _mainCamera.transform.position = newCameraPosition;
                }
            }
        }
    }
}