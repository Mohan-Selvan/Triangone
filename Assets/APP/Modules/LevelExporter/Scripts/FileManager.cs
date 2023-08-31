using System.IO;
using System;

using UnityEngine;

public static class FileManager
{
    public static string ReadFromFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError($"Invalid file path : {filePath}");
            return string.Empty;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File doesn't exist : {filePath}");
            return string.Empty;
        }

        try
        {
            string content = File.ReadAllText(filePath);
            //Debug.Log($"Read successful : {filePath}");
            return content;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading file : {e.Message}");
            return string.Empty;
        }
    }

    public static void WriteToFile(string filePath, string content)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError($"Invalid file path : {filePath}");
            return;
        }

        try
        {
            File.WriteAllText(filePath, content);
            Debug.Log($"Write successful : {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error writing file : {e.Message}");
        }
    }
}