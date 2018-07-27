//from: http://wiki.unity3d.com/index.php?title=Singleton
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;


    public static T Instance
    {
        get
        {
            // -- Make sure there isn't one in the scene already
            instance = instance ?? (FindObjectOfType(typeof(T)) as T);
            // -- create one if we need to
            instance = instance ?? new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
            return instance;
        }
    }

    // -- kill when we close
    private void OnApplicationQuit()
    {
        instance = null;
    }
}