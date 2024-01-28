using UnityEngine;

namespace GGJ.Ingame.Common
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T i { get; protected set; }

        void Awake()
        {
            if (i != null && i != this)
            {
                Destroy(this);
                Debug.LogWarning("An instance of this singleton already exists.");
            }
            else
            {
                i = (T)this;
            }
        }
    }
}