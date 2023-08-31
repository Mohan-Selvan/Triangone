using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

[System.Serializable]
public class LevelData
{
    [JsonProperty("shapes")]
    [field: SerializeField] public List<Shape> Shapes { get; set; }


    #region Static helper functions
    public static string Serialize(LevelData content)
    {
        return JsonConvert.SerializeObject(content, ConverterSettings.GenericSettings);
    }

    public static LevelData Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<LevelData>(message, ConverterSettings.GenericSettings);
    }
    #endregion
}

[System.Serializable]
public class Shape
{
    [JsonProperty("tris")]
    [field: SerializeField] public List<Poly> PolyList;


    #region Static helper functions
    public static string Serialize(Shape content)
    {
        return JsonConvert.SerializeObject(content, ConverterSettings.GenericSettings);
    }

    public static Shape Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<Shape>(message, ConverterSettings.GenericSettings);
    }
    #endregion
}

[System.Serializable]
public class Poly
{
    [JsonProperty("coords")]
    [field: SerializeField] public List<Coord> Coords;

    [JsonProperty("col")]
    [field: SerializeField] public ColorData Color { get; set; }

    public List<Vector2> GetPoints()
    {
        return new List<Vector2>() {
            Coords[0].ToVector2(),
            Coords[1].ToVector2(),
            Coords[2].ToVector2()
        };
    }

    #region Static helper functions
    public static string Serialize(Poly content)
    {
        return JsonConvert.SerializeObject(content, ConverterSettings.GenericSettings);
    }

    public static Poly Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<Poly>(message, ConverterSettings.GenericSettings);
    }
    #endregion
}

[System.Serializable]
public class Coord
{
    [JsonProperty("x")]
    [field: SerializeField] public float X { get; set; }

    [JsonProperty("y")]
    [field: SerializeField] public float Y { get; set; }

    public Vector2 ToVector2()
    {
        return new Vector2(X, Y);
    }
}

[System.Serializable]
public class ColorData
{
    [JsonProperty("r")]
    [field: SerializeField] public float R { get; set; }

    [JsonProperty("g")]
    [field: SerializeField] public float G { get; set; }

    [JsonProperty("b")]
    [field: SerializeField] public float B { get; set; }

    [JsonProperty("a")]
    [field: SerializeField] public float A { get; set; }

    public Color GetColor()
    {
        return new Color(R, G, B, A);
    }

    #region Static helper functions
    public static string Serialize(ColorData content)
    {
        return JsonConvert.SerializeObject(content, ConverterSettings.GenericSettings);
    }

    public static ColorData Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<ColorData>(message, ConverterSettings.GenericSettings);
    }
    #endregion
}
