using System;
using System.Collections.Generic;
using Inventory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace MonstersDataManagement
{
    public abstract class BaseDataClass<T> where T : new()
    {
        private T _cachedValue;
        public void Save(T model)
        {

            PlayerPrefs.SetString(typeof(T).ToString(),JsonConvert.SerializeObject(model));
        }

        public T Load()
        {
            if (_cachedValue != null)
                return _cachedValue;

            _cachedValue = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString( typeof(T).ToString()));

            return _cachedValue;
        }

    }




}