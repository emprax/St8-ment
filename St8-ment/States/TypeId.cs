using System.Threading;

namespace St8Ment.States;

internal static class TypeId
{
    private static int nextId = 0;

    public static int Get<T>() => TypeIdHolder<T>.Id;

    private static class TypeIdHolder<T>
    {
        public static readonly int Id = Interlocked.Increment(ref nextId) - 1;
    }
}