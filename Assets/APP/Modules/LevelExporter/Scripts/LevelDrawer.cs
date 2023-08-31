using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelDrawer : MonoBehaviour
{
    [SerializeField] string filePath = string.Empty;
    [SerializeField] KeyCode exportKey = KeyCode.E;

    [SerializeField]
    public LevelData levelData = new LevelData();


    private void Update()
    {
        if(Input.GetKeyDown(exportKey))
        {
            LevelExporter.ExportLevel(filePath, levelData);
            Debug.Log("Exported level successfully!");
        }
    }
}
