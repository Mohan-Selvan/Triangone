using Newtonsoft.Json;

public static class ConverterSettings
{
    public readonly static JsonSerializerSettings GenericSettings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
    };
}


