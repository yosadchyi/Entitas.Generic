using Entitas;

namespace Entitas.Generic
{
    public class Component<TData>: IComponent
    {
        public TData Data;
    }
}