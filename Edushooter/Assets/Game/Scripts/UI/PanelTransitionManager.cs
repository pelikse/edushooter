using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PanelTransitionManager : MonoBehaviour
{
    public enum Panels
    {
        Lobby,
        CharacterStats,
        Equipment,
        EdushooterSelect,
        Setting,
        EdugymSelect,
        PerkSelect,
        Leaderboards,
        Directory,
        Credits,
        DailyLogin,
    }

    [SerializeField]
    [MMReadOnly]
    private Panels SelectedPanel;

    [SerializeField]
    [MMReadOnly]
    private Panels LastPanel;

    [Space]

    [SerializeField]
    [MMReadOnly]
    private GameObject CurrentPanel;

    [SerializeField]
    [MMReadOnly]
    private GameObject PreviousPanel;

    [Space]
    [Space]
    [Header("Menu Panels")]

    public GameObject Lobby;
    public GameObject CharacterStats;
    public GameObject Equipment;
    //public GameObject EdushooterSelect;
    public GameObject Setting;
    //public GameObject EdugymSelection;
    public GameObject PerkSelection;
    public GameObject Leaderboards;
    public GameObject Directory;
    public GameObject Credits;
    public GameObject DailyLogin;

    [Space]

    [SerializeField] private Panels InitialMenu;

    private GameObject GetPanelGameobject (Panels panel)
    {
        switch (panel)
        {
            case Panels.Lobby:
                return Lobby;

            case Panels.CharacterStats:
                return CharacterStats;

            case Panels.Equipment:
                return Equipment;

            case Panels.Setting:
                return Setting;

            case Panels.PerkSelect:
                return PerkSelection;

            case Panels.Leaderboards:
                return Leaderboards;

            case Panels.Directory:
                return Directory;

            case Panels.Credits:
                return Credits;

            case Panels.DailyLogin:
                return DailyLogin;

            //Unreachable but just in case
            default:
                Debug.LogError("Selected Menu is not within expectations, SelectedMenu is " + SelectedPanel.ToString());
                return GetPanelGameobject(InitialMenu); //returns the initial panel by default
        }
    }

    private void SwitchPanel (Panels panel)
    {
        //the previous panel is the current panel
        LastPanel = SelectedPanel;
        PreviousPanel = GetPanelGameobject(SelectedPanel);

        //the current panel is the new selected panel
        SelectedPanel = panel;
        CurrentPanel = GetPanelGameobject(SelectedPanel);

        //Turn off the previous panel
        PreviousPanel.SetActive(false);

        //Switch on the selected panel
        CurrentPanel.SetActive(true);

        MMGameEvent.Trigger("SwitchedPanel");
    }


#region PanelSwitchFunctions
    public void SwitchToCharacter()
    {
        SwitchPanel(Panels.CharacterStats);
    }

    public void SwitchToLobby()
    {
        SwitchPanel(Panels.Lobby);
    }

    public void SwitchToEdugymSelection()
    {
        SwitchPanel(Panels.EdugymSelect);
    }

    public void SwitchToEquipment()
    {
        SwitchPanel(Panels.Equipment);
    }

    public void SwitchToSetting()
    {
        SwitchPanel(Panels.Setting);
    }

    public void SwitchToLastPanel()
    {
        SwitchPanel(LastPanel);
    }

    public void SwitchToEdushooterSelection()
    {
        SwitchPanel(Panels.EdushooterSelect);
    }
    public void SwitchToPerks()
    {
        SwitchPanel(Panels.PerkSelect);
    }
    
    public void SwitchToLeaderboards()
    {
        SwitchPanel(Panels.Leaderboards);
    }

    public void SwitchToDirectory()
    {
        SwitchPanel(Panels.Directory);
    }

    public void SwitchToCredits()
    {
        SwitchPanel(Panels.Credits);
    }

    public void SwitchToDailyLogin()
    {
        SwitchPanel(Panels.DailyLogin);
    }

    #endregion

    private void Start()
    {
        //Turn off all panels
        Lobby.SetActive(false);
        CharacterStats.SetActive(false);
        Equipment.SetActive(false);
        Setting.SetActive(false);
        PerkSelection.SetActive(false);
        Leaderboards.SetActive(false);
        Directory.SetActive(false);
        Credits.SetActive(false);

        SelectedPanel = InitialMenu;
        SwitchPanel(InitialMenu);
    }
}
