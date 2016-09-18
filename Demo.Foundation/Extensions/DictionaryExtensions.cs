using System;
using System.Collections.Generic;

namespace Demo.Foundation.Extensions
{
    /// <summary>
    /// Provides a series of extension methods for <see cref="IDictionary{TKey, TValue}"/> based off the .NET 4.0 
    /// <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> to which the key should be added.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>
        /// The value for the key. This will be either the existing value for the key
        /// if the key is already in the dictionary, or the new value for the key 
        /// returned by the valueFactory if the key was not in the dictionary.
        /// </returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dictionary.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Adds key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist,
        /// or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/> if the key already exists.
        /// Or updates existing value 
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> to which the key should be added.</param>
        /// <param name="key">The key to be added or whose value should be updated..</param>
        /// <param name="addValueFactory">The function used to generate a value for the absent key.</param>
        /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
        /// <returns>
        /// The new value for the key. This will either be the new value for the key returned by the 
        /// addValueFactory (if the key was absent) or the result of the updateValueFactory (if the key was present).
        /// </returns>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                value = updateValueFactory(key, value);
                dictionary[key] = value;
            }
            else
            {
                value = addValueFactory(key);
                dictionary.Add(key, value);
            }

            return value;
        }
    }

}
