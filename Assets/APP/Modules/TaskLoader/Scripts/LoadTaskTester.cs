using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadTaskTester : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] TaskLoader loader = null;

    [Header("Settings - Input")]
    [SerializeField] KeyCode loadSimpleAsyncTaskKey = KeyCode.Alpha9;
    [SerializeField] KeyCode loadProgressAsyncTaskKey = KeyCode.Alpha0;

    [Header("Testing only")]
    [SerializeField] int counter = 0;

    private void Update()
    {
        if (Input.GetKeyDown(loadSimpleAsyncTaskKey))
        {
            loader.StartLoadTask(new LoadTask()
            {
                Task = SimpleAsynchronousTask(),
                GetProgressNormalizedFunc = null,
                HeadingMessage = "Async success task counting..",
                OnLoadSuccessCallback = () => { Debug.Log("Async success task complete!"); },
                OnLoadFailedCallback = () => { Debug.Log("Async success task failed!"); }
            });
        }
        else if (Input.GetKeyDown(loadProgressAsyncTaskKey))
        {
            loader.StartLoadTask(new LoadTask()
            {
                Task = ProgressAsynchronousTask(),
                GetProgressNormalizedFunc = ()=> { return ((float)counter / 100f); },
                HeadingMessage = "Async success task counting..",
                OnLoadSuccessCallback = () => { Debug.Log("Async success task complete!"); },
                OnLoadFailedCallback = () => { Debug.Log("Async success task failed!"); }
            });
        }
    }

    public async Task SimpleAsynchronousTask()
    {
        Debug.Log("Asynchronous task started");

        //Wait 5 seconds
        await Task.Delay(2000);

        //Has a 50 percent chance of failing.
        if(Random.Range(0, 2) != 0)
        {
            throw new System.Exception("Fail simulation!");
        }

        //Wait 5 seconds
        await Task.Delay(2000);

        Debug.Log("Asynchronous task complete");
    }

    public async Task ProgressAsynchronousTask()
    {
        Debug.Log("Asynchronous task started");

        for(int i = 0; i < 100; i++)
        {
            counter = i;
            await Task.Delay(100);

            if(i == 50)
            {
                //Has a 50 percent chance of failing.
                if (Random.Range(0, 2) != 0)
                {
                    throw new System.Exception("Fail simulation!");
                }
            }
        }

        Debug.Log("Asynchronous task complete");
    }

}
