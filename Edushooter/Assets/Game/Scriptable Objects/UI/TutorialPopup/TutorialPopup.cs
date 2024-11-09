using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialPopup", menuName = "TutorialPopup")]
public class TutorialPopup : ScriptableObject
{
    [SerializeField] private string Title = "TITLE";
    [TextArea(minLines: 4, maxLines: 10)]
    [SerializeField] private string Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
    [Space]
    [SerializeField]
    private VideoClip VideoClip;

    public string PopupTitle { get => Title; }
    public string PopupDesc { get => Description; }
    public VideoClip PopupVideoClip { get => VideoClip; }
}
