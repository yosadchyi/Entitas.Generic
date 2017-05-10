using Entitas;

namespace Entitas.Generic
{
    /// <summary>
    /// ComponentType class holds information about component type id in static property.
    /// </summary>
    /// <typeparam name="TScope">Scope</typeparam>
    /// <typeparam name="TComponent">Component type</typeparam>
    public class ComponentType<TScope, TComponent> where TScope : IScope where TComponent : IComponent
    {
        internal static int Id;
    }
}