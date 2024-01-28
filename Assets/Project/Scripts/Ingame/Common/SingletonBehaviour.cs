using UnityEngine;

namespace GGJ.Ingame.Common
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T instance { get; protected set; }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                Debug.LogWarning("An instance of this singleton already exists.");
            }
            else
            {
                instance = (T)this;
            }
        }
    }
}