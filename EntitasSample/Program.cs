using System;
using Entitas;

namespace Unscientificlab.Entity
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void MulAdd(ref Vector3 addendum, float scale)
        {
            X += addendum.X * scale;
            Y += addendum.Y * scale;
            Z += addendum.Z * scale;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.Y, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator *(Vector3 a, float v)
        {
            return new Vector3(a.X * v, a.Y * v, a.Z * v);
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    [Simulation]
    [ComponentData]
    public struct Position : IComponent
    {
        public Vector3 Location;
        public float Rotation;

        public Position(Vector3 location, float rotation)
        {
            Location = location;
            Rotation = rotation;
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    [Simulation]
    [ComponentData]
    public struct Velocity : IComponent
    {
        public Vector3 Linear;
        public float Angular;

        public Velocity(Vector3 linear, float angular)
        {
            Linear = linear;
            Angular = angular;
        }
    }

    public class MoveSystem : IExecuteSystem
    {
        private readonly IGroup<Entity<Simulation>> _group;

        public MoveSystem(Contexts contexts)
        {
            _group = contexts.Get<Simulation>()
                .GetGroup(
                    Matcher<Entity<Simulation>>.AllOf(
                        Matchers.For<Simulation, Position>(),
                        Matchers.For<Simulation, Velocity>()
                    )
                );
        }

        public void Execute()
        {
            foreach (var entity in _group.GetEntities())
            {
                var position = entity.Get<Position>();
                var velocity = entity.Get<Velocity>();
                const float dt = 1.0f / 60.0f;

                entity.Replace(new Position(position.Location + velocity.Linear * dt,
                    position.Rotation + velocity.Angular * dt));
            }
        }
    }

    internal class Program
    {
        private const int RepeatCount = 10000;

        public static void Main(string[] args)
        {
            // register scopes & components
            ScopeManager.RegisterScopes<Simulation, Input>();
            ComponentTypeManager<Simulation>.Autoscan();

            var contexts = new Contexts(AERCFactories.UnsafeAERCFactory);
            var systems = new Systems().Add(new MoveSystem(contexts));
            var simulationContext = contexts.Get<Simulation>();

            for (var i = 0; i < 1000; i++)
            {
                simulationContext.CreateEntity()
                    .Add(new Position(new Vector3(1, 2, 0), 0))
                    .Add(new Velocity(new Vector3(2, 2, 1), 0.01f));
            }

            // Warm up
            systems.Execute();

            Measure("Entity", () =>
            {
                for (var i = 0; i < RepeatCount; i++)
                    systems.Execute();
            });
        }

        private static void Measure(string name, Action action)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            action();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("[{0}] Avg time: {1}", name, (float) elapsedMs / RepeatCount);
        }
    }
}