using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pixelplacement;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField] SceneLoader sceneLoader = null;

    //PROPERTIES
    public SceneLoader SceneLoader { get => sceneLoader; set => sceneLoader = value; }
}
