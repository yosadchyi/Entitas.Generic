namespace Entitas.Generic
{
    /// <summary>
    /// Generic Entity
    /// </summary>
    /// <typeparam name="TScope">Scope of entity</typeparam>
    public class Entity<TScope> : Entitas.Entity where TScope : IScope
    {
        public Entity<TScope> Add<TComponentData>(TComponentData data)
        {
            var index = ComponentType<TScope, Component<TComponentData>>.Id;
            var component = CreateComponent<Component<TComponentData>>(index);
            component.Data = data;
            AddComponent(index, component);
            return this;
        }

        public Entity<TScope> Replace<TComponentData>(TComponentData data)
        {
            var index = ComponentType<TScope, Component<TComponentData>>.Id;
            var component = CreateComponent<Component<TComponentData>>(index);
            component.Data = data;
            ReplaceComponent(index, component);
            return this;
        }

        public Component<TComponentData> Create<TComponentData>()
        {
            return CreateComponent<Component<TComponentData>>(ComponentType<TScope, Component<TComponentData>>.Id);
        }

        public Entity<TScope> Remove<TComponentData>()
        {
            RemoveComponent(ComponentType<TScope, Component<TComponentData>>.Id);
            return this;
        }

        public TComponentData Get<TComponentData>()
        {
            return ((Component<TComponentData>) GetComponent(ComponentType<TScope, Component<TComponentData>>.Id)).Data;
        }

        public Entity<TScope> SetFlag<TComponentData>(bool flag)
        {
            var hasComponent = Has<TComponentData>();

            if (flag)
            {
                if (hasComponent)
                    return this;
                Add(Create<TComponentData>());
            }
            else
            {
                if (!hasComponent)
                    return this;
                Remove<TComponentData>();
            }
            return this;
        }

        public bool Has<TComponentData>()
        {
            return HasComponent(ComponentType<TScope, Component<TComponentData>>.Id);
        }
    }
}