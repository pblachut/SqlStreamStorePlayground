using System;
using System.Collections.Generic;

namespace SqlStoreTest
{
    public static class TypeCache
    {
        private static readonly Dictionary<string, Type> NameToTypeMapping = new Dictionary<string, Type>();
        private static readonly Dictionary<Type, string> TypeToNameMapping = new Dictionary<Type, string>();

        public static void Add<TEvent>(string name)
        {
            NameToTypeMapping.Add(name, typeof(TEvent));
            TypeToNameMapping.Add(typeof(TEvent), name);
        }

        public static string GetName(Type type) => TypeToNameMapping[type];
        public static Type GetType(string name) => NameToTypeMapping[name];
    }
}