using System.Collections.Generic;
using System.Linq;

namespace VRCFT.Neos
{
    // https://stackoverflow.com/questions/32761880/net-dictionary-with-two-keys-and-one-value
    public class TwoKeyDictionary<Tkey1, Tkey2, TValue>
    {
        private object m_data_lock = new object();
        private Dictionary<Tkey1, Tkey2> m_dic1 = new Dictionary<Tkey1, Tkey2>();
        private Dictionary<Tkey2, TValue> m_dic2 = new Dictionary<Tkey2, TValue>();

        public void Add(Tkey1 key1, Tkey2 key2, TValue value)
        {
            lock (m_data_lock)
            {
                m_dic1[key1] = key2;
                m_dic2[key2] = value;
            }
        }

        public TValue GetByKey1(Tkey1 key1)
        {
            lock (m_data_lock)
                return m_dic2[m_dic1[key1]];
        }

        public TValue GetByKey2(Tkey2 key2)
        {
            lock (m_data_lock)
                return m_dic2[key2];
        }

        public void RemoveByKey1(Tkey1 key1)
        {
            lock (m_data_lock)
            {
                Tkey2 tmp_key2 = m_dic1[key1];
                m_dic1.Remove(key1);
                m_dic2.Remove(tmp_key2);
            }
        }

        public void RemoveByKey2(Tkey2 key2)
        {
            lock (m_data_lock)
            {
                Tkey1 tmp_key1 = m_dic1.First((kvp) => kvp.Value.Equals(key2)).Key;
                m_dic1.Remove(tmp_key1);
                m_dic2.Remove(key2);
            }
        }

        public bool ContainsKey1(Tkey1 key1)
        {
            lock (m_data_lock)
                return m_dic1.ContainsKey(key1);
        }

        public bool ContainsKey2(Tkey2 key2)
        {
            lock (m_data_lock)
                return m_dic2.ContainsKey(key2);
        }
    }
}
