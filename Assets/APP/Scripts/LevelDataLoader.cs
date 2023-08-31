using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelDataLoader
{
    public static LevelData LoadLevelFromFile(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"Invalid path : {path}");
            return null;
        }

        string content = FileManager.ReadFromFile(path);
        if (string.IsNullOrEmpty(content))
        {
            Debug.LogError($"Level data invalid");
            return null;
        }

        LevelData levelData = LevelData.Deserialize(content);
        return levelData;
    }
}
