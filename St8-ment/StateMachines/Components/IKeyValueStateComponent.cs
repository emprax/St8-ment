using System;

namespace St8Ment.StateMachines.Components
{
    public interface IKeyValueStateComponent<TKey> : IStateComponent where TKey : IEquatable<TKey>
    {
        void Add(TKey key, IStateComponent component);

        bool TryGetValue(TKey key, out IStateComponent component);
    }
}
