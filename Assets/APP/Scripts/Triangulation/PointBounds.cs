using UnityEngine;

public class PointBounds
{
    private float _minX;
    private float _minY;
    private float _maxX;
    private float _maxY;

    public Vector2 TopLeft => new Vector2(_minX, _maxY);
    public Vector2 TopRight => new Vector2(_maxX, _maxY);
    public Vector2 BottomLeft => new Vector2(_minX, _minY);
    public Vector2 BottomRight => new Vector2(_maxX, _minY);

    public float minX { get => _minX; set => _minX = value; }
    public float minY { get => _minY; set => _minY = value; }
    public float maxX { get => _maxX; set => _maxX = value; }
    public float maxY { get => _maxY; set => _maxY = value; }

    public PointBounds(float minX, float maxX, float minY, float maxY)
    {
        this._minX = minX;
        this._minY = minY;
        this._maxX = maxX;
        this._maxY = maxY;
    }
}
