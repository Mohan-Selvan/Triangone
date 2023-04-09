using System.Threading.Tasks;
using UnityEngine;

public class LoadTask
{
    public System.Func<Task> TaskProvider;
    public System.Action OnLoadSuccessCallback;
    public System.Action OnLoadFailedCallback;
    public System.Func<float> GetProgressNormalizedFunc;

    //UI Items
    public string HeadingMessage = string.Empty;
}

public class LoadAsyncOperation
{
    public System.Func<AsyncOperation> Operation;
    public System.Action OnLoadSuccessCallback;
    public System.Action OnLoadFailedCallback;

    //UI Items
    public string HeadingMessage = string.Empty;
}
