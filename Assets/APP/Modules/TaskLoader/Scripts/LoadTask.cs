using System.Threading.Tasks;
using UnityEngine;

public class LoadTask
{
    public Task Task;
    public System.Action OnLoadSuccessCallback;
    public System.Action OnLoadFailedCallback;
    public System.Func<float> GetProgressNormalizedFunc;

    //UI Items
    public string HeadingMessage = string.Empty;
}

public class LoadAsyncOperation
{
    public AsyncOperation Operation;
    public System.Action OnLoadSuccessCallback;
    public System.Action OnLoadFailedCallback;

    //UI Items
    public string HeadingMessage = string.Empty;
}
