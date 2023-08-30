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

            GameWorld gameWorld = GameWorld.Instance;

            string sceneName = Constants.SCENE_GAME;

            GameWorld.Instance.TaskLoader.StartLoadTask(new LoadAsyncOperation()
            {
                Operation = () => { return gameWorld.SceneLoader.GetSceneLoadOperation(sceneName); },
                HeadingMessage = "Loading...",
                OnLoadFailedCallback = () => { Debug.Log("Scene load failed!"); },
                OnLoadSuccessCallback = () => { 
                    Debug.Log("Scene load complete!");
                    GameWorld.Instance.GameManager.StartGame();
                }
            });
        });
    }
}
