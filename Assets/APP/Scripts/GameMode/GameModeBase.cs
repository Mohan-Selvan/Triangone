using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBase : MonoBehaviour
{
    public virtual string GameMode_PrettyName { get; }

    internal virtual IEnumerator InitializeLevel()
    {
        yield return null;
    }

    internal virtual IEnumerator StartLevel()
    {
        yield return null;
    }
}
