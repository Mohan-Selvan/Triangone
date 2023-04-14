using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] TaskLoader taskLoader = null;

    public void TryLoadScene(string sceneName)
    {
        taskLoader.StartLoadTask(new LoadAsyncOperation()
        {
            Operation = ()=> { return GetSceneLoadOperation(sceneName); },
            HeadingMessage = "Loading...",
            OnLoadSuccessCallback = () => { Debug.Log("Scene load complete!"); },
            OnLoadFailedCallback = () => { Debug.Log("Scene load failed!"); }
        });
    }

    public void TryLoadScene(LoadAsyncOperation loadSceneOperation)
    {
        taskLoader.StartLoadTask(loadSceneOperation);
    }

    public AsyncOperation GetSceneLoadOperation(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName);
    }
}
