using System.Numerics;
using VRCFT.Neos;

namespace VRCFTNeos.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TwoKeyDictionary<string, string, float> twoKeyDictionary = new TwoKeyDictionary<string, string, float>();

            // Test add methods
            Console.WriteLine(twoKeyDictionary.Add("key1", "key2", 1.0f)); // True
            Console.WriteLine(twoKeyDictionary.Add("key3", "key4", 1.0f)); // True
            Console.WriteLine(twoKeyDictionary.Add("key5", "key6", 1.0f)); // True
            Console.WriteLine(twoKeyDictionary.Add("key5", "key7", 1.0f)); // False, key5 exists as a Key1
            Console.WriteLine(twoKeyDictionary.Add("key6", "key6", 1.0f)); // False, key6 exists as a Key2
            Console.WriteLine(twoKeyDictionary.ContainsValue(1.0f)); // True
            Console.WriteLine();

            // Test set methods
            Console.WriteLine(twoKeyDictionary.SetByPair("key1", "key2", 2.0f)); //True
            Console.WriteLine(twoKeyDictionary.SetByKey1("key1", 2.0f)); // True
            Console.WriteLine(twoKeyDictionary.SetByKey2("key2", 3.0f)); // True
            Console.WriteLine();

            // Test get methods
            Console.WriteLine(twoKeyDictionary.GetByKey1("key1")); // 3.0f
            float value;
            Console.WriteLine(twoKeyDictionary.TryGetByKey1("key2", out value)); // False, 0f
            Console.WriteLine(twoKeyDictionary.GetByKey1("key3")); // 1.0f
            Console.WriteLine();

            // Test remove methods. Count is 3 prior
            twoKeyDictionary.RemoveByKey1("key1"); // 2
            twoKeyDictionary.RemoveByKey1("key2"); // Does not exist, so 2

            // Test count methods
            Console.WriteLine(twoKeyDictionary.Count); 
            Console.WriteLine();

            // Test contains methods
            Console.WriteLine(twoKeyDictionary.ContainsKey1("key1")); // False, we just removed it
            Console.WriteLine(twoKeyDictionary.ContainsKey2("key2")); // False. This was tied to key1!
            Console.WriteLine(twoKeyDictionary.ContainsKey1("key3")); // True
            Console.WriteLine(twoKeyDictionary.ContainsKey1("key5")); // True
            Console.WriteLine(twoKeyDictionary.ContainsKey1("key4")); // False, this never existed in the first place!
            Console.WriteLine();

            // Test clear methods
            twoKeyDictionary.Clear();
            Console.WriteLine(twoKeyDictionary.Count); //0
            Console.WriteLine();
        }
    }
}