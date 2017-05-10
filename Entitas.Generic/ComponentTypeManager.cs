using System;
using System.Collections.Generic;
using System.Reflection;
using Entitas;

// ReSharper disable StaticMemberInGenericType
namespace Entitas.Generic
{
    /// <summary>
    /// Component type manager: manages component types per scope
    /// </summary>
    /// <typeparam name="TScope">Scope</typeparam>
    public class ComponentTypeManager<TScope> where TScope : IScope
    {
        private static int _lastId;
        private static readonly List<Type> _registeredTypes = new List<Type>();
        private static string[] _componentNamesCache;
        private static Type[] _typesCache;

        public static void Autoscan()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!IsComponentData(type) || !IsInScope(type))
                        continue;

                    Register(type);
                }
            }
        }

        private static void Register(Type dataType)
        {
            var componentType = typeof(ComponentType<,>);
            var genericType = componentType.MakeGenericType(typeof(TScope), MakeGenericComponentType(dataType));

            if (_registeredTypes.Contains(genericType))
                return;

            _registeredTypes.Add(genericType);

            var fieldInfo = genericType.GetField("Id", BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);

            if (fieldInfo == null)
                throw new Exception(string.Format("Type `{0}' does not contains `Id' field", genericType.Name));

            fieldInfo.SetValue(null, _lastId);
            _lastId++;
        }

        private static Type MakeGenericComponentType(Type dataType)
        {
            return typeof(Component<>).MakeGenericType(dataType);
        }

        private static bool IsInScope(MemberInfo type)
        {
            foreach (var t in type.GetCustomAttributes(false))
            {
                if (t is TScope)
                    return true;
            }
            return false;
        }

        private static bool IsComponentData(MemberInfo type)
        {
            foreach (var t in type.GetCustomAttributes(false))
            {
                if (t is ComponentData)
                    return true;
            }
            return false;
        }

        public static void Register<TComponentData>()
        {
            var type = typeof(TComponentData);

            if (_registeredTypes.Contains(type))
                return;

            _registeredTypes.Add(type);
            ComponentType<TScope, Component<TComponentData>>.Id = _lastId++;
        }

        public static int TotalComponents
        {
            get { return _registeredTypes.Count; }
        }

        public static string[] ComponentNamesCache
        {
            get
            {
                if (_componentNamesCache == null)
                {
                    _componentNamesCache = new string[_registeredTypes.Count];

                    for (var i = 0; i < _componentNamesCache.Length; i++)
                    {
                        _componentNamesCache[i] = _registeredTypes[i].Name;
                    }
                }
                return _componentNamesCache;
            }
        }

        public static Type[] ComponentTypesCache
        {
            get
            {
                if (_typesCache == null)
                {
                    _typesCache = _registeredTypes.ToArray();
                }
                return _typesCache;
            }
        }
    }
}