#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace dgames.Utils
{
    public abstract class BaseUnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Dictionary<Type, T> instances = new Dictionary<Type, T>();
        public static T? Instance => instances.GetValueOrDefault(typeof(T));

        protected bool TryInitializeInstance()
        {
            if (!instances.ContainsKey(typeof(T)))
            {
                instances.Add(typeof(T), this as T ?? throw new NullReferenceException($"{this} is null !"));
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return false;
            }

            return true;
        }
    }
}
