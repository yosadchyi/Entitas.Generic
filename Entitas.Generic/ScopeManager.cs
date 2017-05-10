using System;
using Entitas;

namespace Entitas.Generic
{
    public class ScopeManager
    {
        public const int MaxScopes = 32;

        private static int _lastId;
        private static readonly Type[] _contextTypes;
        private static readonly Func<Func<IEntity, IAERC>, IContext>[] _factories;

        public static int Count { get { return _lastId; } }

        static ScopeManager()
        {
            _contextTypes = new Type[MaxScopes];
            _factories = new Func<Func<IEntity, IAERC>, IContext>[MaxScopes];
        }

        public static IContext CreateContext(int index, Func<IEntity, IAERC> aercFactory)
        {
            return _factories[index](aercFactory);
        }

        public static void RegisterScope<TScope>() where TScope : IScope
        {
            var scopeType = typeof(TScope);

            if (IsScopeRegistered(scopeType))
                return;

            _contextTypes[_lastId] = typeof(ScopedContext<TScope>);
            _factories[_lastId] = (aercFactory) => new ScopedContext<TScope>(aercFactory);
            ScopeType<TScope>.Id = _lastId;
            _lastId++;
        }

        private static bool IsScopeRegistered(Type type)
        {
            for (var i = 0; i < _lastId; i++)
            {
                if (type == _contextTypes[i])
                    return true;
            }
            return false;
        }

        public static void RegisterScopes<TS1, TS2>() where TS1: IScope where TS2: IScope
        {
            RegisterScope<TS1>();
            RegisterScope<TS2>();
        }

        public static void RegisterScopes<TS1, TS2, TS3>() where TS1: IScope where TS2: IScope where TS3: IScope
        {
            RegisterScope<TS1>();
            RegisterScope<TS2>();
            RegisterScope<TS3>();
        }

        public static void RegisterScopes<TS1, TS2, TS3, TS4>() where TS1: IScope where TS2: IScope where TS3: IScope where TS4: IScope
        {
            RegisterScope<TS1>();
            RegisterScope<TS2>();
            RegisterScope<TS3>();
            RegisterScope<TS4>();
        }

        public static void RegisterScopes<TS1, TS2, TS3, TS4, TS5>() where TS1: IScope where TS2: IScope where TS3: IScope where TS4: IScope where TS5: IScope
        {
            RegisterScope<TS1>();
            RegisterScope<TS2>();
            RegisterScope<TS3>();
            RegisterScope<TS4>();
            RegisterScope<TS5>();
        }
    }
}