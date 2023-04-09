using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

public class TaskLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TaskLoaderUI loaderUI;

    private Coroutine _loadRoutine = null;

    private void Start()
    {
        //Disabling loaderUI at the start
        loaderUI.DisableLoaderUI();
    }

    internal void StartLoadTask(LoadTask loadTask)
    {
        if(_loadRoutine != null)
        {
            Debug.LogError("Another load is in progress, Ignoring load request");
            return;
        }

        _loadRoutine = StartCoroutine(LoadRoutine(loadTask));
    }

    internal void StartLoadTask(LoadAsyncOperation loadOperation)
    {
        if (_loadRoutine != null)
        {
            Debug.LogError("Another load is in progress, Ignoring load request");
            return;
        }

        _loadRoutine = StartCoroutine(LoadAsyncOperationRoutine(loadOperation));
    }

    private IEnumerator LoadRoutine(LoadTask loadTask)
    {
        float loadBarRefreshDuration = 0.2f;
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(loadBarRefreshDuration);

        bool trackProgress = loadTask.GetProgressNormalizedFunc != null;
        float progress = 0f;

        Task task = loadTask.Task;

        bool run = true;

        loaderUI.ShowLoaderUI(headingMessage: loadTask.HeadingMessage,trackProgress);

        while (run)
        {
            if (trackProgress)
            {
                progress = loadTask.GetProgressNormalizedFunc();
                loaderUI.UpdateProgress(progress);
            }


            yield return wait;

            run = !(task.IsCompleted || task.IsFaulted || task.IsCanceled);

            if (task.IsCompletedSuccessfully)
            {
                loaderUI.UpdateProgress(1f);
            }
        }

        if (task.IsCompletedSuccessfully)
        {
            Debug.Log($"Load task completed successfully : {task.Id}");
            loadTask.OnLoadSuccessCallback?.Invoke();
        }
        else
        {
            Debug.LogError($"Load task failed : {task.Id}");
            loadTask.OnLoadFailedCallback?.Invoke();
        }

        loaderUI.DisableLoaderUI();

        _loadRoutine = null;
    }

    private IEnumerator LoadAsyncOperationRoutine(LoadAsyncOperation loadTask)
    {
        float loadBarRefreshDuration = 0.2f;
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(loadBarRefreshDuration);

        float progress = 0f;

        AsyncOperation operation = loadTask.Operation;

        bool run = true;

        loaderUI.ShowLoaderUI(headingMessage: loadTask.HeadingMessage, trackProgress: false);

        while (run)
        {
            progress = operation.progress;
            loaderUI.UpdateProgress(progress);

            yield return wait;

            run = !(operation.isDone);
        }

        loaderUI.UpdateProgress(1f);

        if (operation.isDone)
        {
            Debug.Log($"Load operation completed successfully");
            loadTask.OnLoadSuccessCallback?.Invoke();
        }
        else
        {
            Debug.LogError($"Load operation failed");
            loadTask.OnLoadFailedCallback?.Invoke();
        }

        loaderUI.DisableLoaderUI();

        _loadRoutine = null;
    }
}
