using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "CustomObjects", menuName = "Edugym Description")]
public class EdugymDescription : ScriptableObject
{
    [MMInformation("Ini isinya informasi singkat tentang Edugym yg mau ditampilkan ke player. Level_Name HARUS PERSIS SAMA dengan nama Scene yang bersangkutan.", MMInformationAttribute.InformationType.None, false)]
    [SerializeField] private string GameTitle = "EDUGYM";
    [SerializeField] private VideoClip GameDemo;
    [TextArea(minLines: 4, maxLines: 10)]
    [SerializeField] private string Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
    [SerializeField] private string Level_Name = "";

    public string Title { get => GameTitle; }
    public string Desc { get => Description; }
    public string LevelName { get => Level_Name; set => Level_Name = value; }
    public VideoClip GameplayDemo { get => GameDemo; set => GameDemo = value; }
}
