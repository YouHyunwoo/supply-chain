using System.Collections.Generic;
using SupplyChain.View.World;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SupplyChain.Model
{
    public class Transporter : Tool
    {
        [SerializeField] private Carrier _carrier;

        public Carrier Carrier => _carrier;

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
            _carrier.enabled = false;
        }

        private void OnDisable()
        {
            if (_carrier != null)
                _carrier.enabled = false;
        }

        private void Update()
        {
            if (IsPointerOverUI()) return;

            if (Input.GetMouseButtonDown(0))
            {
                _carrier.enabled = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _carrier.enabled = false;
            }
        }
    }
}