using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    [SerializeField] private float _x;
    [SerializeField] private float _y;

    public Vector2 Position => ToVector2();

    public float x { get => _x; }
    public float y { get => _y; }

    public Point(float x, float y)
    {
        this._x = x;
        this._y = y;
    }

    public Point(Vector2 vec)
    {
        this._x = vec.x;
        this._y = vec.y;
    }

    public void SetPosition(Vector2 vector)
    {
        this._x = vector.x;
        this._y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(_x, _y);
    }
}
