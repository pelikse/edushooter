using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    public void SaveGameSettings()
    {
        //save the sound settings
        MMSoundManager.Instance.SaveSettings();
    }

    public void SetMasterVolume(float volume)
    {
        MMSoundManager.Instance.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, volume);
    }

    public void SetMusicVolume(float volume)
    {
        MMSoundManager.Instance.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, volume);
    }

    public void SetSFXVolume(float volume)
    {
        MMSoundManager.Instance.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, volume);
    }

    public void SetUIVolume(float volume)
    {
        MMSoundManager.Instance.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.UI, volume);
    }
}
