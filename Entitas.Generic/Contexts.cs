using System;
using Entitas;

namespace Entitas.Generic
{
    public class Contexts
    {
        private readonly IContext[] _contexts;

        public Contexts(Func<IEntity, IAERC> aercFactory)
        {
            _contexts = new IContext[ScopeManager.Count];

            for (var i = 0; i < ScopeManager.Count; i++)
            {
                _contexts[i] = ScopeManager.CreateContext(i, aercFactory);
            }
        }

        public ScopedContext<TScope> Get<TScope>() where TScope : IScope
        {
            return (ScopedContext<TScope>) _contexts[ScopeType<TScope>.Id];
        }
    }
}