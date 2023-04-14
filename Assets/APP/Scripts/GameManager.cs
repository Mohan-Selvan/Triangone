using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pixelplacement;

public enum GameState
{
    STARTING,
    RUNNING, 
    PAUSED,
    ENDED
}

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PointField levelHandler = null;

    [Header("Testing only")]
    [SerializeField] GameState currentGameState = default;
    [SerializeField] GameState previousGameState = default;

    //PROPERTIES
    public GameState CurrentGameState { get => currentGameState; private set => currentGameState = value; }
    public GameState PreviousGameState { get => previousGameState; private set => previousGameState = value; }

    //EVENTS
    public event System.Action<GameState> OnGameStateChanged = null;

    internal void SwitchGameState(GameState targetGameState)
    {
        if(currentGameState == targetGameState)
        {
            Debug.Log($"Re-entry, State : {currentGameState}");
            //return;
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

            case GameState.ENDED:

                break;

            default:
                Debug.LogError($"Unhandled gamestate : {targetGameState}");
                break;
        }

        OnGameStateChanged?.Invoke(currentGameState);
    }

    public void StartGame()
    {
        StartCoroutine(StartGame_Routine());
    }

    public void PauseGame()
    {
        if(currentGameState != GameState.RUNNING)
        {
            Debug.LogError($"Game not in expected state, Expected state : {GameState.RUNNING}, CurrentState : {currentGameState}");
        }

        SwitchGameState(GameState.PAUSED);
    }

    public void ResumeGame()
    {
        if (currentGameState != GameState.PAUSED)
        {
            Debug.LogError($"Game not in expected state, Expected state : {GameState.PAUSED}, CurrentState : {currentGameState}");
        }

        SwitchGameState(GameState.RUNNING);
    }

    public void EndGame()
    {
        Debug.Log("Ending game..");
        GameWorld.Instance.SceneLoader.GetSceneLoadOperation(Constants.SCENE_HOME);
    }

    public void HandleLevelComplete()
    {
        if (currentGameState != GameState.RUNNING)
        {
            Debug.LogError($"Game not in expected state, Expected state : {GameState.RUNNING}, CurrentState : {currentGameState}");
        }

        SwitchGameState(GameState.ENDED);

        Debug.Log("Level complete");

        //TODO :: Remove

        GameWorld gameWorld = GameWorld.Instance;

        string sceneName = Constants.SCENE_HOME;

        gameWorld.TaskLoader.StartLoadTask(new LoadAsyncOperation()
        {
            Operation = () => { return gameWorld.SceneLoader.GetSceneLoadOperation(sceneName); },
            HeadingMessage = "Loading...",
            OnLoadFailedCallback = () => { Debug.Log("Scene load failed!"); },
            OnLoadSuccessCallback = () => {
                Debug.Log("Scene load complete!");
                gameWorld.GameManager.StartGame();
            }
        });
    }

    public IEnumerator StartGame_Routine()
    {
        if (levelHandler == null)
        {
            levelHandler = FindObjectOfType<PointField>();
        }

        if (levelHandler == null)
        {
            Debug.LogError($"{nameof(PointField)} not present in scene!");
            yield break;
        }

        SwitchGameState(GameState.STARTING);

        yield return levelHandler.DrawLevel_Routine();

        SwitchGameState(GameState.RUNNING);
    }
}
