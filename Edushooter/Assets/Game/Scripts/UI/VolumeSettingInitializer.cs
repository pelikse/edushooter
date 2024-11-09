using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettingInitializer : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public Slider Master;
    public Slider Music;
    public Slider SFX;
    public Slider UI;


    //implement event listener
    private void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }


    public void OnMMEvent(MMGameEvent eventType)
    {
        //fetch the current volume and import it into the sliders
        if (eventType.EventName.Equals("SwitchedPanel") || eventType.EventName.Equals("InitializeVolumeSettings"))
        {
            MMSoundManagerSettingsSO setting = MMSoundManager.Current.settingsSo;
            Debug.Log("initializing volume settings");

            Master.value = setting.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master);
            Music.value = setting.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music);
            SFX.value = setting.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx);
            UI.value = setting.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.UI);
        }
    }
}
