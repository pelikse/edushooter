using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class UpgradeCostFetcher : MonoBehaviour, DataFetcher, MMEventListener<MMGameEvent>
{
    public TextMeshProUGUI HealthCost;
    public TextMeshProUGUI DamageCost;
    public TextMeshProUGUI SpeedCost;
    public TextMeshProUGUI DashCost;
    public TextMeshProUGUI ReloadCost;

    [Space, Space]

    public TextMeshProUGUI HealthUpgrade;
    public TextMeshProUGUI DamageUpgrade;
    public TextMeshProUGUI SpeedUpgrade;
    public TextMeshProUGUI DashUpgrade;
    public TextMeshProUGUI ReloadUpgrade;

    [Space, Space]

    [Header("Upgrade Info Description")]
    [TextArea(3,3)]
    public string InfoDesc = "{0:0} > <color=green>{1:0}</color>";

    [TextArea(3, 3)]
    public string PercentDesc = "{0:0}% > <color=green>{1:0}%</color>";

    [TextArea(3, 3)]
    public string CapDesc = "<color=#ffc803>{0:0}</color>";

    [Space, Space]

    [Header("Managers")]
    public CharacterUpgradeManager UpgradeManager;
    public EdushooterStatManager StatManager;
    [Space]
    public Character PlayerPrefab;

    private int _levelCap;

    public void FetchData(PlayerExternalData data)
    {
        // get the cost of each upgrade
        HealthCost.text = UpgradeManager.GetUpgradeCostString(data.EdushooterStats.Health);
        DamageCost.text = UpgradeManager.GetUpgradeCostString(data.EdushooterStats.Damage);
        SpeedCost.text = UpgradeManager.GetUpgradeCostString(data.EdushooterStats.Speed);
        DashCost.text = UpgradeManager.GetUpgradeCostString(data.EdushooterStats.Dash);
        ReloadCost.text = UpgradeManager.GetUpgradeCostString(data.EdushooterStats.Reload);

        // display the upgrade info
        FetchUpgradeInfo();
    }

    private void FetchUpgradeInfo()
    {
        //get the latest level cap
        _levelCap = ProgressManager.TryGetInstance().LevelCap;

        // if its not a player prefab
        if (!PlayerPrefab.CharacterType.Equals(Character.CharacterTypes.Player))
        {
            Debug.LogError("No player prefab assigned to upgrade cost fetcher");
            return;
        }

        PlayerEdushooterData data = ProgressManager.TryGetInstance().ExternalData.EdushooterStats;

        //insert hp info
        if (PlayerPrefab.gameObject.TryGetComponent<Health>(out var _playerHealth))
        {
            CalculateUpgradeStats(data.Health, _playerHealth.MaximumHealth, StatManager.GetHealthMultiplier(data.Health), StatManager.GetHealthMultiplier(data.Health + 1), HealthUpgrade, InfoDesc);
        }

        //insert speed info
        if (PlayerPrefab.gameObject.TryGetComponent<CharacterMovement>(out var _playerMovement))
        {
            CalculateUpgradeStats(data.Speed, _playerMovement.WalkSpeed * 10f, StatManager.GetSpeedMultiplier(data.Speed), StatManager.GetSpeedMultiplier(data.Speed + 1), SpeedUpgrade, InfoDesc);
        }

        //insert damage info
        CalculateUpgradeStats(data.Damage, 100f, StatManager.GetDamageMultiplier(data.Damage), StatManager.GetDamageMultiplier(data.Damage + 1), DamageUpgrade, PercentDesc);

        //insert dash info
        CalculateUpgradeStats(data.Dash, 100f, StatManager.GetDashMultiplier(data.Dash), StatManager.GetDashMultiplier(data.Dash + 1), DashUpgrade, PercentDesc);

        //insert reload info
        CalculateUpgradeStats(data.Reload, 100f, StatManager.GetReloadMultiplier(data.Reload), StatManager.GetReloadMultiplier(data.Reload + 1), ReloadUpgrade, PercentDesc);
    }

    private void CalculateUpgradeStats(int level, float initial_value, float multiplier1, float multiplier2, TextMeshProUGUI tmp, string normal_format)
    {
        string _currentValue = Mathf.Ceil(initial_value * multiplier1).ToString();
        string _nextValue = Mathf.Ceil(initial_value * multiplier2).ToString();

        if (level < _levelCap) tmp.text = string.Format(normal_format, _currentValue, _nextValue);
        else tmp.text = string.Format(CapDesc, _currentValue);
    }



    private void Start()
    {
        _levelCap = ProgressManager.TryGetInstance().LevelCap;
    }

    #region EventListener Implement
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName.Equals("SwitchedPanel") || eventType.EventName.Equals("CharacterStatChanged"))
        {
            FetchData(ProgressManager.TryGetInstance().ExternalData);
        }
    }
    #endregion
}
