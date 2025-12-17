using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }

    public Test Test;
    public Map Map;
    public HUD HUD;
    public CameraMovement CameraMovement;
    public float Money;
    public int LinkCount;
    private PlayerMode _currentMode = PlayerMode.Normal;

    public PlayerMode Mode => _currentMode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("[Main] 게임 시작");

        Test.OnStart();
        Map.Create();
        Map.CreateNodes();
        CameraMovement.SetPosition(Map.Center);
        HUD.SetMoneyText(Money);
        HUD.SetLinkCountText(LinkCount);

        Debug.Log("[Main] 초기 자금: " + Money);
        Debug.Log("[Main] 초기 링크 수: " + LinkCount);
    }

    public void SetMode(PlayerMode mode)
    {
        _currentMode = mode;
        switch (mode)
        {
            case PlayerMode.Normal:
                break;
            case PlayerMode.TransferCargo:
                break;
        }
    }

    public void SellResourceCargo(float price)
    {
        Money += price;
        HUD.SetMoneyText(Money);
    }

    public void BuyLink()
    {
        if (Money >= 15)
        {
            Money -= 15;
            LinkCount += 1;
            HUD.SetMoneyText(Money);
            HUD.SetLinkCountText(LinkCount);
        }
    }
}
