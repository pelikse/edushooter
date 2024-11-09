using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelSwitch : MonoBehaviour
{
    [System.Serializable]
    public enum ModelType
    {
        Roller,
        Sniper,
        Exploder,
        Medic,
        Charger,
        Ghost
    }

    public GameObject Roller;
    public GameObject Sniper;
    public GameObject Exploder;
    public GameObject Medic;
    public GameObject Charger;
    public GameObject Ghost;

    [Space]

    [SerializeField]
    [MMReadOnly]
    private GameObject CurrentModel;

    [SerializeField]
    [MMReadOnly]
    private ModelType ModelDisplay = ModelType.Roller;

    public void _Initialize(ModelType initialModel)
    {
        TurnOffAllModels();
        GameObject _setModel;
        ModelDisplay = initialModel;

        // turn on the chosen model
        switch (initialModel)
        {
            case ModelType.Roller:
                _setModel = Roller;
                break;
            case ModelType.Sniper:
                _setModel = Sniper;
                break;
            case ModelType.Exploder:
                _setModel = Exploder;
                break;
            case ModelType.Medic:
                _setModel = Medic;
                break;
            case ModelType.Charger:
                _setModel = Charger;
                break;
            case ModelType.Ghost:
                _setModel = Ghost;
                break;
            default:
                _setModel = Roller;
                ModelDisplay = ModelType.Roller;
                break;
        }

        CurrentModel = _setModel;
        CurrentModel.SetActive(true);
    }

    private void TurnOffAllModels()
    {
        Roller.SetActive(false);
        Sniper.SetActive(false);
        Exploder.SetActive(false);
        Medic.SetActive(false);
        Charger.SetActive(false);
        Ghost.SetActive(false);
    }

    public void SwitchModel(ModelType model)
    {
        // if we're switching to the same model then dont do anything
        if (model == ModelDisplay)
        {
            return;
        }


        // turn off all models
        TurnOffAllModels();

        GameObject _setModel;
        ModelDisplay = model;

        // turn on the chosen model
        switch (model)
        {
            case ModelType.Roller:
                _setModel = Roller;
                break;
            case ModelType.Sniper: 
                _setModel= Sniper;
                break;
            case ModelType.Exploder:
                _setModel = Exploder; 
                break;
            case ModelType.Medic:
                _setModel = Medic; 
                break;
            case ModelType.Charger:
                _setModel = Charger; 
                break;
            case ModelType.Ghost:
                _setModel = Ghost;
                break;
            default:
                _setModel = Roller;
                ModelDisplay = ModelType.Roller;
                break;
        }

        CurrentModel = _setModel;
        CurrentModel.SetActive(true);
    }
}
