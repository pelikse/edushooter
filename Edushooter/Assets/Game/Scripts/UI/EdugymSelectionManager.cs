using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdugymSelectionManager : MonoBehaviour
{
    [MMInformation("This manager handles changing the displayed Edugym information based on user selection and proper scene transitions using the EdugymDescription scriptable object.", MMInformationAttribute.InformationType.None, false)]
    [Space]

    [SerializeField] private EdugymDescription InitialPick;

    [Space]
    [Header("Info Box")]
    // Objects to insert the information to
    [SerializeField] private LevelSelector LevelSelector;
    [SerializeField] private GameInfoFetcher GameInfoFetcher;


    public void StartSelectedGame()
    {
        if (LevelSelector.LevelName != "")
        {
            LevelSelector.GoToLevel();
        }
    }

    public void SetSelectedGame(EdugymDescription info)
    {
        GameInfoFetcher.FetchEdugymInfo(info);
        LevelSelector.LevelName = info.LevelName;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (InitialPick != null)
        {
            GameInfoFetcher.FetchEdugymInfo(InitialPick);
            SetSelectedGame(InitialPick);
        }
    }
}
