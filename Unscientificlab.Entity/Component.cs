using Entitas;

namespace Unscientificlab.Entity
{
    public class Component<TData>: IComponent
    {
        public TData Data;
    }
}