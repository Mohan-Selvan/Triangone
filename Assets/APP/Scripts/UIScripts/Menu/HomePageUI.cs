using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageUI : MonoBehaviour
{
    [Header("References - UI")]
    [SerializeField] Button playButton = null;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Debug.Log("Loading Main scene");
            GameWorld.Instance.SceneLoader.TryLoadScene("Main");
        });
    }
}
