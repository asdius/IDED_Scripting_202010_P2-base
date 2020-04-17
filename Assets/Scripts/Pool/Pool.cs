using UnityEngine;

namespace Pools
{
    public abstract class Pool<T> : MonoBehaviour where T : Pool<T>
    {
        public static T Instance = default;

        [Tooltip("Campi, Number of objects to instantiate")]
        [SerializeField] protected int initialInstances = 10;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }

            OnAwake();
        }

        protected abstract void OnAwake();

        protected abstract void InitializePool();
    } 
}
