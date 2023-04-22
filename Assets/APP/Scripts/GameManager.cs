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

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PointField pointField = null;
    [SerializeField] RingHandler ringHandler = null;

    [Header("Settigs - Input")]
    [SerializeField] KeyCode drawLevelKey = KeyCode.Alpha0;
    [SerializeField] KeyCode updateScaleKey = KeyCode.Alpha9;

    [Header("Debug only")]
    [SerializeField] bool continuousRefresh = false;


    [Header("Testing only")]
    [SerializeField] GameState currentGameState = default;
    [SerializeField] GameState previousGameState = default;

    //PROPERTIES
    public GameState CurrentGameState { get => currentGameState; private set => currentGameState = value; }
    public GameState PreviousGameState { get => previousGameState; private set => previousGameState = value; }

    //EVENTS
    public event System.Action<GameState> OnGameStateChanged = null;


    private void Update()
    {
        if (Input.GetKeyDown(drawLevelKey))
        {
            Debug.Log($"{nameof(GameManager)} : {nameof(StartGame)}");
            StartGame();
        }

        if (Input.GetKeyDown(updateScaleKey) || continuousRefresh)
        {
            Debug.Log($"{nameof(PointField)} : {nameof(pointField.UpdateScale)}");
            pointField.UpdateScale();
        }
    }

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

            case GameState.ENDING:

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
        StartCoroutine(EndGame_Routine());
    }

    public void HandleLevelComplete()
    {
        if (currentGameState != GameState.RUNNING)
        {
            Debug.LogError($"Game not in expected state, Expected state : {GameState.RUNNING}, CurrentState : {currentGameState}");
        }

        SwitchGameState(GameState.ENDED);

        Debug.Log("Level complete");

        StartGame();
    }

    public IEnumerator StartGame_Routine()
    {
        Debug.Log($"{nameof(StartGame_Routine)} : Start");

        if (pointField == null)
        {
            pointField = FindObjectOfType<PointField>();
        }

        if (pointField == null)
        {
            Debug.LogError($"{nameof(PointField)} not present in scene!");
            yield break;
        }

        if (ringHandler == null)
        {
            ringHandler = FindObjectOfType<RingHandler>();
        }

        if (ringHandler == null)
        {
            Debug.LogError($"{nameof(RingHandler)} not present in scene!");
            yield break;
        }

        SwitchGameState(GameState.STARTING);

        ringHandler.Initialize();
        ringHandler.SetWallSafeState_All(isSafe: true, animate: true);

        yield return pointField.DrawLevel_Routine();

        SwitchGameState(GameState.RUNNING);

        Debug.Log($"{nameof(StartGame_Routine)} : Complete");
    }

    public IEnumerator EndGame_Routine()
    {
        Debug.Log($"{nameof(EndGame_Routine)} : Start");

        SwitchGameState(GameState.ENDING);

        yield return new WaitForSeconds(4f);

        GameWorld.Instance.SceneLoader.TryLoadScene(Constants.SCENE_HOME);

        SwitchGameState(GameState.ENDED);

        Debug.Log($"{nameof(EndGame_Routine)} : Complete");
    }
}
