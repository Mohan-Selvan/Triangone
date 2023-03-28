using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    [SerializeField] float x;
    [SerializeField] float y;

    public Vector2 Position => ToVector2();

    public float X { get => x; }
    public float Y { get => y; }

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Point(Vector2 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
    }

    public void SetPosition(Vector2 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
