using Cysharp.Threading.Tasks;
using SupplyChain.Enum;
using UnityEngine;

namespace SupplyChain.Model
{
    public class MainSystem : MonoBehaviour
    {
        public Test Test;
        public GameManager GameManager;
        public ToolManager ToolManager;
        public Player Player;
        public UpgradeManager UpgradeManager;

        private void Awake()
        {
            Globals.MainSystem = this;
        }

        private async UniTaskVoid Start()
        {
            // 게임 초기화: SetUp - 독립적인 행위
            Test.SetUp();
            GameManager.SetUp();
            ToolManager.SetUp();
            Player.SetUp();
            Globals.MainView.SetUp();

            // 테스트: 바로 게임 시작
            await StartGame();
        }

        // public async UniTask StartIntro()
        // {
            
        // }

        public async UniTask StartGame()
        {
            Globals.MainView.LockScreen();
            await Globals.MainView.FadeOut();
            GameManager.CreateGame();
            GameManager.StartInitialLevel();
            // Globals.MainView.ShowLevelView();
            await Globals.MainView.FadeIn();
            Globals.MainView.UnlockScreen();
        }

        public bool BuyTool(int toolIndex, int toolCost)
        {
            if (Player.Money < toolCost)
            {
                Debug.LogWarning("[Main] 자금이 부족합니다. 현재 자금: " + Player.Money + ", 필요 자금: " + toolCost);
                return false;
            }

            Player.Money -= toolCost;
            Globals.MainView.LevelView.SetMoney(Player.Money);
            ToolManager.Unlock(toolIndex);

            Debug.Log("[Main] 도구 구매 완료: " + toolIndex + ", 남은 자금: " + Player.Money);
            return true;
        }

        public void SelectProcessNode()
        {
            Debug.Log("[Main] 공정 노드 선택 처리");
            ToolManager.Select(ToolType.Locator);
        }
    }    
}
