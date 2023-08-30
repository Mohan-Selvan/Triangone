using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameModeBase currentGameMode = null;

    private Coroutine _activeRoutine = null;

    IEnumerator Start()
    {
        Debug.Log("Starting..");
        yield return new WaitForSeconds(1f);

        StartGame();
    }

    internal void StartGame()
    {
        if(currentGameMode == null)
        {
            Debug.LogError("Game mode not set");
            return;
        }

        StopCoroutineIfNotNull();

        _activeRoutine = StartCoroutine(StartGame_Routine());
    }

    private IEnumerator StartGame_Routine()
    {
        yield return currentGameMode.InitializeLevel();

        yield return currentGameMode.StartLevel();

        _activeRoutine = null;
    }

    private void StopCoroutineIfNotNull()
    {
        if(_activeRoutine != null)
        {
            Debug.Log("Stopping existing routine");
            StopCoroutine(_activeRoutine);
        }

        _activeRoutine = null;
    }
}
