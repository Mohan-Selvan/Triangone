using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] TaskLoader taskLoader = null;

    public void TryLoadScene(string sceneName)
    {
        taskLoader.StartLoadTask(new LoadAsyncOperation()
        {
            Operation = ()=> { return LoadSceneAsync(sceneName); },
            HeadingMessage = "Loading...",
            OnLoadSuccessCallback = () => { Debug.Log("Scene load complete!"); },
            OnLoadFailedCallback = () => { Debug.Log("Scene load failed!"); }
        });
    }

    private AsyncOperation LoadSceneAsync(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName);
    }
}
