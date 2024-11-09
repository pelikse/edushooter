using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFillBar : MonoBehaviour
{
    [Serializable]
    public enum Direction
    {
        BottomTop
    }

    public SpriteRenderer Renderer;
    public Direction FillDirection = Direction.BottomTop;

    private float _startingWidth, _startingHeight;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        if (Renderer == null)
        {
            Debug.LogError("No sprite renderer found on a sprite fill bar!");
        }

        _startingHeight = Renderer.size.y;
        _startingWidth = Renderer.size.x;
    }

    public void UpdateFill(float _currentValue, float _maxValue)
    {
        Vector2 _size = Renderer.size;
        Renderer.size = new Vector2(_startingWidth, (_currentValue/_maxValue) * _startingHeight);
    }
}
