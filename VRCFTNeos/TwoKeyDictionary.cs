using System.Collections.Generic;
using System.Linq;

namespace VRCFT.Neos
{
    //  https://stackoverflow.com/questions/32761880/net-dictionary-with-two-keys-and-one-value

    /// <summary>
    /// A dictionary whose values can be accessed by two keys. Enforces unique outer keys and inner keys
    /// </summary>
    /// <remarks>
    ///  This isn't the fastest implementation, but it's best suited to adapt to varying OSC protocol naming schemes
    /// </remarks>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class TwoKeyDictionary<TKey1, TKey2, TValue> where TValue : struct
    {
        private object m_data_lock = new object();
        private Dictionary<TKey1, TKey2> m_dic1 = new Dictionary<TKey1, TKey2>();
        private Dictionary<TKey2, TValue> m_dic2 = new Dictionary<TKey2, TValue>();

        /// <summary>
        ///   Adds the specified key and value to the dictionary.
        ///   This will always add the value if the input parameters are not null.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        /// <returns>Returns true if the value was added, false if the value was already present in the dictionary.</returns>
        public bool Add(TKey1 key1, TKey2 key2, TValue value)
        {
            lock (m_data_lock)
            {
                if (key1 == null || key2 == null || ContainsKey1(key1) || ContainsKey2(key2))
                {
                    return false;
                }

                m_dic1[key1] = key2;
                m_dic2[key2] = value;

                return true;
            }
        }

        /// <summary>
        /// Sets the specified key and value to the dictionary, if it already exists.
        /// This is functionally identical to the Add() method, but it will not add the value if it doesn't already exist.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        /// <returns>Returns true if the value was set, false if the value was not found.</returns>
        public bool Set(TKey1 key1, TKey2 key2, TValue value)
        {
            lock (m_data_lock)
            {
                if (!(ContainsKey1(key1) || ContainsKey2(key2)))
                {
                    return false;
                }

                m_dic1[key1] = key2;
                m_dic2[key2] = value;
                return true;
            }
        }

        /// <summary>
        /// Sets the specified value in the dictionary given the first key, if it already exists.
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="value"></param>
        /// <returns>Returns true if the value was set, false if the value was not found.</returns>
        public bool SetByKey1(TKey1 key1, TValue value)
        {
            lock (m_data_lock)
            {
                if (!ContainsKey1(key1))
                {
                    return false;
                }

                m_dic2[m_dic1[key1]] = value;
                return true;
            }
        }

        /// <summary>
        /// Sets the specified value in the dictionary given the second key, if it already exists.
        /// </summary>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        /// <returns>Returns true if the value was set, false if the value was not found.</returns>
        public bool SetByKey2(TKey2 key2, TValue value)
        {
            lock (m_data_lock)
            {
                if (!ContainsKey2(key2))
                {
                    return false;
                }

                m_dic2[key2] = value;
                return true;
            }
        }

        // In the event we try and get a TValue that does not exist, return null.
        // I opted for this instead of default(T) as in my use case TValues will be predominantly floats.
        // Returning 0.0f if the value is not found wouldn't make sense, at least for me.

        /// <summary>
        /// Gets the value associated with the outer key. If no value is found, null is returned.
        /// </summary>
        /// <param name="key1"></param>
        /// <returns>The TValue for this Tkey1. If no value is found, null is returned. </returns>
        public TValue? GetByKey1(TKey1 key1)
        {
            lock (m_data_lock)
            {
                if (!m_dic1.TryGetValue(key1, out TKey2 key2))
                {
                    return null;
                }

                if (m_dic2.TryGetValue(key2, out TValue value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the value associated with the inner key. If no value is found, null is returned.
        /// </summary>
        /// <param name="key2"></param>
        /// <returns>The TValue for this Tkey2. If no value is found, null is returned. </returns>
        public TValue? GetByKey2(TKey2 key2)
        {
            lock (m_data_lock)
            {
                if (m_dic2.TryGetValue(key2, out TValue value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Removes the value associated with the outer key. If the outer key is not found, nothing happens. If the outer key is found, the inner key must also exist and is also removed.
        /// </summary>
        /// <param name="key1"></param>
        public bool RemoveByKey1(TKey1 key1)
        {
            lock (m_data_lock)
            {
                if (!m_dic1.TryGetValue(key1, out TKey2 tmp_key2))
                {
                    return false;
                }

                m_dic1.Remove(key1);
                m_dic2.Remove(tmp_key2);
                return true;
            }
        }

        /// <summary>
        /// Removes the value associated with the inner key. If the inner key is not found, nothing happens. If the inner key is found, the outer key must also exist and is also removed.
        /// </summary>
        /// <param name="key2"></param>
        public bool RemoveByKey2(TKey2 key2)
        {
            lock (m_data_lock)
            {
                if (!m_dic2.ContainsKey(key2))
                {
                    return false;
                }

                TKey1 tmp_key1 = m_dic1.First((kvp) => kvp.Value.Equals(key2)).Key;
                m_dic1.Remove(tmp_key1);
                m_dic2.Remove(key2);
                return true;
            }
        }

        /// <summary>
        /// Get the length of this two-key dictionary. m_dic1.Count is used, as it is the same as m_dic2.Count.
        /// </summary>
        public int Count
        {
            get
            {
                lock (m_data_lock)
                {
                    return m_dic1.Count;
                }
            }
        }

        /// <summary>
        /// Evaluates whether the two-key dictionary contains the outer key.
        /// </summary>
        /// <param name="key1"></param>
        /// <returns>True if the outer dictionary contains the key.</returns>
        public bool ContainsKey1(TKey1 key1)
        {
            lock (m_data_lock)
            {
                return m_dic1.ContainsKey(key1);
            }
        }

        /// <summary>
        /// Evaluates whether the two-key dictionary contains the inner key.
        /// </summary>
        /// <param name="key2"></param>
        /// <returns>True if the inner dictionary contains the key.</returns>
        public bool ContainsKey2(TKey2 key2)
        {
            lock (m_data_lock)
            {
                return m_dic2.ContainsKey(key2);
            }
        }

        /// <summary>
        /// Evaluates whether the two-key dictionary contains the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the two-key dictionary contains the value.</returns>
        public bool ContainsValue(TValue value)
        {
            lock (m_data_lock)
            {
                return m_dic2.ContainsValue(value);
            }
        }

        /// <summary>
        /// Clears the two-key dictionary.
        /// </summary>
        public void Clear()
        {
            lock (m_data_lock)
            {
                m_dic1.Clear();
                m_dic2.Clear();
            }
        }
    }
}
