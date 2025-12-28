using UnityEngine;

namespace SupplyChain.Model
{
    public class Game
    {
        public Level[] Levels;
        public Level CurrentLevel { get; private set; }

        // public Player player;

        public void Create(Data.Game gameData)
        {
            var levelDataArray = gameData.Levels;
            Levels = new Level[levelDataArray.Length];
            for (int i = 0; i < levelDataArray.Length; i++)
            {
                var levelData = levelDataArray[i];
                var levelIndex = i;
                var level = new Level
                {
                    Data = levelData,
                    Index = levelIndex,
                };

                level.Create();
                Levels[i] = level;
            }
        }

        public void StartLevel(int levelIndex)
        {
            if (Levels == null || levelIndex < 0 || levelIndex >= Levels.Length)
            {
                Debug.LogError("[Game] 잘못된 레벨 인덱스: " + levelIndex);
                return;
            }

            CurrentLevel = Levels[levelIndex];
            CurrentLevel.SetUp();
        }

        public void EndLevel(bool isSuccess)
        {
            if (isSuccess)
                HandleLevelSucceeded();
            else
                HandleLevelFailed();

            CurrentLevel.TearDown();
            CurrentLevel = null;
        }

        private void HandleLevelSucceeded()
        {
            Debug.Log("[Game] 레벨 성공 처리");
            // 클리어 결과 보여주기
            // -> 확인 버튼 클릭: 페이드 -> Region 제거

            // Globals.MainSystem.Player.SetMode(ToolType.None);
            // CameraMovement.SetPosition(Vector2.zero);
            // Globals.MainView.LevelView.Hide();
            // Globals.MainView.LaboratoryView.Show();
            // Globals.MainView.LaboratoryView.UpgradeView.gameObject.SetActive(true);
            // Globals.MainView.LaboratoryView.MapView.gameObject.SetActive(false);
            // Globals.MainView.LaboratoryView.SetMoney(Globals.MainSystem.Player.Money);
            // Globals.MainView.StageWorldView.Hide();
            // Globals.MainView.LaboratoryWorldView.Show();
        }

        private void HandleLevelFailed()
        {
            Debug.Log("[Game] 레벨 실패 처리");

            Debug.Log("[StageManager] 목표 실패!");
            // 페이드

            // Globals.MainSystem.Player.SetMode(ToolType.None);
            // CameraMovement.SetPosition(Vector2.zero);
            // Globals.MainView.LevelView.Hide();
            // Globals.MainView.LaboratoryView.Show();
            // Globals.MainView.LaboratoryView.UpgradeView.gameObject.SetActive(true);
            // Globals.MainView.LaboratoryView.MapView.gameObject.SetActive(false);
            // Globals.MainView.LaboratoryView.SetMoney(Globals.MainSystem.Player.Money);
            // Globals.MainView.StageWorldView.Hide();
            // Globals.MainView.LaboratoryWorldView.Show();
        }
    }
}