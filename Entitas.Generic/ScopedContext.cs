using System;
using Entitas;

namespace Entitas.Generic
{
    /// <summary>
    /// Context for given scope
    /// </summary>
    /// <typeparam name="TScope"></typeparam>
    public class ScopedContext<TScope> : Context<Entity<TScope>> where TScope : IScope
    {
        public ScopedContext(Func<IEntity, IAERC> aercFactory) : base(
            ComponentTypeManager<TScope>.TotalComponents,
            0,
            new ContextInfo(
                typeof(TScope).Name,
                ComponentTypeManager<TScope>.ComponentNamesCache,
                ComponentTypeManager<TScope>.ComponentTypesCache
            ),
            aercFactory)
        {
        }
    }
}