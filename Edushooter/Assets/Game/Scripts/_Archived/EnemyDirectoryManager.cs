using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyDirectoryManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [System.Serializable]
    public class EnemyDirectory
    {
        public EnemyDirectoryEntry[] Enemies;
    }


    public EnemyDirectoryEntry InitialEnemy;
    public EnemyDirectory Entries;

    [Space]

    public EnemyModelSwitch ModelSwitcher;

    [Space, Space]

    public TextMeshProUGUI EnemyText;
    public TextMeshProUGUI EnemyDesc;
    public TextMeshProUGUI EnemyTip;

    [Space]

    public TextMeshProUGUI EnemyHealth;
    public TextMeshProUGUI EnemyDamage;
    public TextMeshProUGUI EnemySpeed;

    private void _Initialize()
    {
        Debug.Log("initializing enemy directory");
        ModelSwitcher._Initialize(InitialEnemy.Enemy);
        ApplyEnemyDetails(InitialEnemy);
    }

    private void SetEnemyEntry(EnemyModelSwitch.ModelType enemy)
    {
        foreach(EnemyDirectoryEntry type in Entries.Enemies)
        {
            if (type.Enemy == enemy)
            {
                ApplyEnemyDetails(type);
                return;
            }
        }
        //if it isn't in the entry, just set it to roller
        Debug.Log("enemy type not found, setting to default....");
        ApplyEnemyDetails(InitialEnemy);
    }

    private void ApplyEnemyDetails(EnemyDirectoryEntry entry)
    {
        ModelSwitcher.SwitchModel(entry.Enemy);

        EnemyText.text = entry.Enemy.ToString();
        EnemyDesc.text = entry.EnemyDescription;
        EnemyTip.text = entry.EnemyTips;

        EnemyHealth.text = entry.Health.ToString();
        EnemyDamage.text = entry.Damage.ToString();
        EnemySpeed.text = entry.Speed.ToString();
    }


    #region EnemySwitches

    public void SwitchToRoller()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Roller);
    }

    public void SwitchToSniper()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Sniper);
    }

    public void SwitchToExploder()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Exploder);
    }

    public void SwitchToMedic()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Medic);
    }

    public void SwitchToCharger()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Charger);
    }

    public void SwitchToGhost()
    {
        SetEnemyEntry(EnemyModelSwitch.ModelType.Ghost);
    }


    #endregion


    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName.Equals("SwitchedPanel"))
        {
            _Initialize();
        }
    }
    #endregion
}
