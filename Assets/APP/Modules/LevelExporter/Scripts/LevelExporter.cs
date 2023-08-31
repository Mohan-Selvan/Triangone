using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelExporter
{
    internal static void ExportLevel(string filePath, LevelData levelData)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError($"Invalid file path : {filePath}");
        }

        string content = LevelData.Serialize(levelData);
        FileManager.WriteToFile(filePath, content);

        Debug.Log($"File written successfully to path : {filePath}");
    }
}
