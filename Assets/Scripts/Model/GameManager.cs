using SupplyChain.Enum;
using SupplyChain.View.World;
using UnityEngine;

namespace SupplyChain.Model
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TerrainFactory _terrainFactory;
        [SerializeField] private NodeFactory _nodeFactory;
        [SerializeField] private FogFactory _fogFactory;

        public CameraMovement CameraMovement;
        public Game Game = null;

        private void Update()
        {
            if (Game != null && Game.CurrentLevel != null)
            {
                Game.CurrentLevel.Update();
                var timer = Game.CurrentLevel.Timer;
                var duration = Game.CurrentLevel.Duration;
                Debug.Log("[Level] 시간 경과: " + timer + " / " + duration);
                Globals.MainView.LevelView.Timer.SetTimeRatio((duration - timer) / duration);
                if (timer >= duration)
                    Globals.MainSystem.GameManager.EndLevel(false);
            }
        }

        public void SetUp()
        {
            Game = null;
        }

        public void CreateGame()
        {
            Game = new Game();
            Game.Create(Globals.Database.Game);
        }

        public void DeleteGame()
        {
            Game = null;
        }

        #region Level
        public bool StartInitialLevel()
        {
            return StartLevel(0);
        }

        public bool StartLevel(int levelIndex)
        {
            Debug.Log("[Main] 레벨 시작: " + levelIndex);

            // UI 처리: 화면 전환 등
            Globals.MainView.LaboratoryView.Hide();
            Globals.MainView.LevelView.Show();
            Globals.MainView.LaboratoryWorldView.Hide();
            Globals.MainView.StageWorldView.Show();

            var toolManager = Globals.MainSystem.ToolManager;

            Game.StartLevel(levelIndex);

            // UI 처리: 레벨 뷰 표시 등
            CameraMovement.SetPosition(Game.CurrentLevel.Center);
            Globals.MainView.LevelView.SetMoney(Globals.MainSystem.Player.Money);
            Globals.MainView.LevelView.ToolSlotList.SetUpToolSlots(Globals.Database.Tools, toolManager.HandleToolSelected);

            toolManager.Select(ToolType.Transporter);

            return true;
        }

        public void EndLevel(bool isSuccess)
        {
            Debug.Log("[Main] 레벨 종료: " + (isSuccess ? "성공" : "실패"));

            Game.EndLevel(isSuccess);

            // UI 처리: 결과 화면 표시 등
        }

        public void SellCargo(int price)
        {
            var actualPrice = Mathf.RoundToInt(price * Globals.MainSystem.Player.EarningsMultiplier);
            Game.CurrentLevel.EarnMoney(actualPrice);
            Globals.MainSystem.Player.Money += actualPrice;
            Globals.MainView.LevelView.SetMoney(Globals.MainSystem.Player.Money);
            if (Game.CurrentLevel.CheckObjectives())
            {
                Debug.Log("[Region] 목표 수익 달성: " + Game.CurrentLevel.Earnings + " / " + Game.CurrentLevel.Data.ObjectiveEarnings);
                EndLevel(true);
            }
        }
        #endregion
    }
}