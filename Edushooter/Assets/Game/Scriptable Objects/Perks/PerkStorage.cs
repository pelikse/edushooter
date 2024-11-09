using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkObject", menuName = "Perks Storage")]
public class PerkStorage : ScriptableObject
{
    [System.Serializable]
    public class PerkGroup
    {
        public string Name;
        public PerkScriptable[] PerkList;

        [MMInformation("The probability that a perk from a certain rarity will be chosen is the perk's frequency/total frequency", MMInformationAttribute.InformationType.None, true)]
        [SerializeField]
        public float Frequency;

        public PerkScriptable GetPerkFromGroup()
        {
            if (PerkList == null) return null;

            return PerkList[Random.Range(0, PerkList.Length)];
        }
    }

    [Header("Perks")]
    public PerkGroup[] PerkGroups;

    [MMInformation("Each PerkGroup member should have at least one perk in each groups to prevent an error when retrieving perks.", MMInformationAttribute.InformationType.Warning, false)]
    [SerializeField]
    [MMReadOnly]
    public bool _Warning = false;

    public PerkScriptable GetPerk()
    {
        // get the total frequency of each group
        float _total = 0f;

        foreach (PerkGroup perk in PerkGroups)
        {
            _total += perk.Frequency;
        }

        if (_total == 0)
        {
            Debug.LogError("Total frequency is 0 in the perk selector, check the Perk Groups!");
            return null;
        }

        float _chosenFrequency = Random.Range(0, _total);
        float _cumulativeFrequency = 0f;

        for (int i = 0; i < PerkGroups.Length; i++)
        {
            _cumulativeFrequency += PerkGroups[i].Frequency;

            if (_chosenFrequency < _cumulativeFrequency)
            {
                return PerkGroups[i].GetPerkFromGroup();
            }
        }

        //a failsafe if no perks are selected (shouldn't be reachable)
        return PerkGroups[0].GetPerkFromGroup();
    }
}
