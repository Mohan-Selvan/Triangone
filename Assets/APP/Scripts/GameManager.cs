using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pixelplacement;

public enum GameState
{
    STARTING,
    RUNNING, 
    PAUSED,
    ENDING,
    ENDED
}

public class GameManager : Singleton<GameManager>
{
    public GameState currentGameState = default;

    public void SwitchGameState(GameState targetGameState)
    {
        if(currentGameState == targetGameState)
        {
            Debug.LogError($"Ignoring re-entry, State : {currentGameState}");
            return;
        }

        GameState previousGameState = currentGameState;
        currentGameState = targetGameState;


        switch (targetGameState)
        {
            case GameState.STARTING:

                break;

            case GameState.RUNNING:

                break;

            case GameState.PAUSED:

                break;

            case GameState.ENDING:

                break;

            case GameState.ENDED:

                break;

            default:
                Debug.LogError($"Unhandled gamestate : {targetGameState}");
                break;
        }
    }

    internal void StartGame()
    {
        
    }
}
