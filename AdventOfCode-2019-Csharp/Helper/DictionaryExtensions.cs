﻿using System;
using System.Collections.Generic;

namespace AdventOfCode_2019_Csharp.Helper
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (key == null || !dictionary.ContainsKey(key))
                return default;

            return dictionary[key];
        }

        public static TValue GetOrAddExisting<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (key == null)
                return default; 
            if(!dictionary.ContainsKey(key))
                dictionary.Add(key, value);

            return dictionary[key];
        }

        public static void AddOrUpdateExisting<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        public static IDictionary<TKey, TValue> DeepCopy<TKey, TValue>(this Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            var ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            foreach (var (key, value) in original)
            {
                ret.Add(key, (TValue)value.Clone());
            }
            return ret;
        }

        public static void PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
        }
    }
}
