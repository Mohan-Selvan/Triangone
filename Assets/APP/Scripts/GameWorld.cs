using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pixelplacement;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] GameManager gameManager = null;
    [SerializeField] TaskLoader taskLoader = null;
    [SerializeField] SceneLoader sceneLoader = null;
    [SerializeField] UIManager uiManager = null;

    //PROPERTIES
    public SceneLoader SceneLoader { get => sceneLoader; }
    public TaskLoader TaskLoader { get => taskLoader; }
    public GameManager GameManager { get => gameManager; }
    public UIManager UIManager { get => uiManager; }
}
