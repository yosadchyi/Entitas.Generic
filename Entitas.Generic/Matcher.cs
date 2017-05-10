using Entitas;

namespace Entitas.Generic
{
    public class Matcher<TScope, TComponentData> where TScope: IScope
    {
        private static IMatcher<Entity<TScope>> _cached;

        public static IMatcher<Entity<TScope>> Instance
        {
            get
            {
                if (_cached == null)
                {
                    var matcher =
                        (Matcher<Entity<TScope>>) Matcher<Entity<TScope>>
                            .AllOf(ComponentType<TScope, Component<TComponentData>>.Id);

                    matcher.componentNames = ComponentTypeManager<TScope>.ComponentNamesCache;
                    _cached = matcher;
                }
                return _cached;
            }
        }
    }

    public class Matchers
    {
        public static IMatcher<Entity<TScope>> For<TScope, TComponentData>() where TScope: IScope
        {
            return Matcher<TScope, TComponentData>.Instance;
        }
    }
}