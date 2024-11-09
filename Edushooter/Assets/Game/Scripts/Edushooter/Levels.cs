using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    // the limit on levels that a player can achieve
    public int LevelCap = 10;
    // amount of exp required to initially level up
    public float InitialExpThreshold = 10f;
    // the multiplier that ExpThreshold should increase per level
    public float ExpMultiplier = 1.25f;

    [Space]
    [Space]
    // the level that a player has currently
    [SerializeField]
    [MMReadOnly]
    private int _level = 1;

    // exp that the player has currently
    [SerializeField]
    [MMReadOnly]
    private float _currentExp = 0f;

    // the exp threshold for the current level
    [SerializeField]
    [MMReadOnly]
    private float _currentExpThreshold = 10f;

    
    // call to make a player level up
    public void LevelUp()
    {
        // add level, consume exp required
        _level++;
        _currentExp -= _currentExpThreshold;

        // increase the ExpThreshold for the next level
        _currentExpThreshold *= ExpMultiplier;
    }

    // call to receive Exp
    public void ReceiveExp(float exp)
    {
        // if the level cap is reached, do not receive exp anymore
        if (_level >= LevelCap)
        {
            return;
        }

        // else, add exp and check for level up
        _currentExp += exp;

        while (_currentExp >= _currentExpThreshold)
        {
            LevelUp();
        }
    }

    private void Start()
    {
        _currentExpThreshold = InitialExpThreshold;
    }
}
