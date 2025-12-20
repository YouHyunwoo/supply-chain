using UnityEngine;

namespace SupplyChain.Model
{
    public class MainSystem : MonoBehaviour
    {
        public Test Test;
        public StageManager StageManager;
        public ToolManager ToolManager;
        public Player Player;

        private void Awake()
        {
            Globals.MainSystem = this;
        }

        private void Start()
        {
            // 게임 초기화: SetUp - 독립적인 행위
            Test.SetUp();
            StageManager.SetUp();
            ToolManager.SetUp();
            Globals.MainView.SetUp();

            // 초기 세팅: 의존적 행위
            StageManager.StartStage(0);
        }

        public void SellResourceCargo(int price)
        {
            var actualPrice = Mathf.RoundToInt(price * Player.EarningsMultiplier);
            Player.Money += actualPrice;
            Globals.MainSystem.StageManager.Region.AddEarnings(actualPrice);
            Globals.MainView.StageView.SetMoneyText(Player.Money);
        }

        public bool BuyTool(int toolIndex, int toolCost)
        {
            if (Player.Money < toolCost)
            {
                Debug.LogWarning("[Main] 자금이 부족합니다. 현재 자금: " + Player.Money + ", 필요 자금: " + toolCost);
                return false;
            }

            Player.Money -= toolCost;
            Globals.MainView.StageView.SetMoneyText(Player.Money);
            ToolManager.Unlock(toolIndex);

            Debug.Log("[Main] 도구 구매 완료: " + toolIndex + ", 남은 자금: " + Player.Money);
            return true;
        }
    }    
}
