using SupplyChain.View.World;
using UnityEngine;

namespace SupplyChain.Model
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private Region _regionPrefab;
        public CameraMovement CameraMovement;

        private Region _region;

        public Region Region => _region;

        public void SetUp()
        {
            
        }

        public void StartStage(int regionIndex)
        {
            Globals.MainView.LaboratoryView.Hide();
            Globals.MainView.StageView.Show();
            Globals.MainView.LaboratoryWorldView.Hide();
            Globals.MainView.StageWorldView.Show();

            var toolManager = Globals.MainSystem.ToolManager;

            var regionData = LoadRegionData(regionIndex);
            if (regionData == null)
            {
                Debug.LogError("[StageManager] 스테이지 데이터를 불러올 수 없습니다: " + regionIndex);
                return;
            }

            var region = Instantiate(_regionPrefab);
            region.Create(regionData);
            _region = region;

            CameraMovement.SetPosition(Region.Center);
            Globals.MainView.StageView.SetMoney(Globals.MainSystem.Player.Money);
            Globals.MainView.StageView.ToolSlotList.SetUpToolSlots(Globals.Database.Tools, toolManager.HandleToolSelected);

            toolManager.Select(ToolType.Transporter);
        }

        public Data.Region LoadRegionData(int stageIndex)
        {
            if (0 <= stageIndex && stageIndex < Globals.Database.Regions.Length)
            {
                return Globals.Database.Regions[stageIndex];
            }
            else
            {
                Debug.LogError("[StageManager] 잘못된 스테이지 인덱스: " + stageIndex);
                return null;
            }
        }

        public void AchieveObjective()
        {
            Debug.Log("[StageManager] 목표 달성!");
            // 클리어 결과 보여주기
            // -> 확인 버튼 클릭: 페이드 -> Region 제거
            if (_region != null)
            {
                Destroy(_region.gameObject);
                _region = null;
            }

            Globals.MainSystem.Player.SetMode(ToolType.None);
            CameraMovement.SetPosition(Vector2.zero);
            Globals.MainView.StageView.Hide();
            Globals.MainView.LaboratoryView.Show();
            Globals.MainView.LaboratoryView.UpgradeView.gameObject.SetActive(true);
            Globals.MainView.LaboratoryView.MapView.gameObject.SetActive(false);
            Globals.MainView.LaboratoryView.SetMoney(Globals.MainSystem.Player.Money);
            Globals.MainView.StageWorldView.Hide();
            Globals.MainView.LaboratoryWorldView.Show();
        }

        public void FailObjective()
        {
            Debug.Log("[StageManager] 목표 실패!");
            // 페이드
            if (_region != null)
            {
                Destroy(_region.gameObject);
                _region = null;
            }

            Globals.MainSystem.Player.SetMode(ToolType.None);
            CameraMovement.SetPosition(Vector2.zero);
            Globals.MainView.StageView.Hide();
            Globals.MainView.LaboratoryView.Show();
            Globals.MainView.LaboratoryView.UpgradeView.gameObject.SetActive(true);
            Globals.MainView.LaboratoryView.MapView.gameObject.SetActive(false);
            Globals.MainView.LaboratoryView.SetMoney(Globals.MainSystem.Player.Money);
            Globals.MainView.StageWorldView.Hide();
            Globals.MainView.LaboratoryWorldView.Show();
        }

        public void HandleStageStarted(int stageIndex)
        {
            StartStage(stageIndex);
        }
    }
}