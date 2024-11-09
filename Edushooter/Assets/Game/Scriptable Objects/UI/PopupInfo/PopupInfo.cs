using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomObjects", menuName = "PopupInfo")]
public class PopupInfo : ScriptableObject
{
    [SerializeField] private string Title = "TITLE";
    [TextArea(minLines: 4, maxLines: 10)]
    [SerializeField] private string Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    public string PopupTitle { get => Title;}
    public string PopupDesc { get => Description;}
}
