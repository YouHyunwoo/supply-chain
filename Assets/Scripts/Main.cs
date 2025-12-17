using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Instance { get; private set; }

    public Map Map;
    public HUD HUD;
    public CameraMovement CameraMovement;

    private PlayerMode _currentMode = PlayerMode.Normal;
    private float _money = 14.0f;
    private int _links;

    public PlayerMode Mode => _currentMode;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Map.Create();
        Map.CreateNodes();

        CameraMovement.SetPosition(Map.Center);

        HUD.SetMoneyText(_money);
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
        _money += price;
        HUD.SetMoneyText(_money);
    }

    public void BuyLink()
    {
        if (_money >= 15)
        {
            _money -= 15;
            _links += 1;
            HUD.SetMoneyText(_money);
            HUD.SetLinksText(_links);
        }
    }
}
