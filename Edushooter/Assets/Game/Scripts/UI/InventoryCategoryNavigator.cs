using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCategoryNavigator : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public enum InventoryType
    {
        WeaponInventory,
        HeadInventory,
        BodyInventory,
    }

    [System.Serializable]
    public class InventoryPanels
    {
        public InventoryType type;
        public GameObject panel;
    }

    [SerializeField]
    [MMReadOnly]
    private InventoryType CurrentType = InventoryType.WeaponInventory;

    public InventoryType InitialType = InventoryType.WeaponInventory;

    [Space, Space]

    [MMInformation("Always have one of each inventory type assigned, and PRECISELY ONE. More than that will break the script.", MMInformationAttribute.InformationType.Warning, true)]
    public InventoryPanels[] Panels = new InventoryPanels[3];

    private void TurnOffAllPanels()
    {
        foreach (var p in Panels)
        {
            p.panel.SetActive(false);
        }
    }

    private void SwitchInventoryPanel(InventoryType panelType)
    {
        foreach (var p in Panels)
        {
            if (p.type == panelType)
            {
                p.panel.SetActive(true);
            }
        }

        MMGameEvent.Trigger("SwitchInventoryPanel");
    }

    private void SwitchInventoryType(InventoryType type)
    {
        CurrentType = type;

        TurnOffAllPanels();
        SwitchInventoryPanel(CurrentType);

        MMGameEvent.Trigger("SwitchedEquipmentTab");
    }

    #region Switchers

    public void SwitchToWeapon()
    {
        SwitchInventoryType(InventoryType.WeaponInventory);
    }

    public void SwitchToHead()
    {
        SwitchInventoryType(InventoryType.HeadInventory);
    }

    public void SwitchToBody()
    {
        SwitchInventoryType(InventoryType.BodyInventory);
    }

    #endregion

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
        if (eventType.EventName.Equals("SwitchedPanel"))
        {
            SwitchInventoryType(InitialType);
        }
    }
    #endregion
}
