using System.Collections.Generic;

namespace System.Collections.Immutable
{
    public class ImmutableHashSetSurrogate<T> : ImmutableSurrogate, ISet<T>
    {
        private ImmutableHashSet<T>.Builder builder;

        public ImmutableHashSetSurrogate()
        {
            this.builder = ImmutableHashSet.CreateBuilder<T>();
        }

        public ImmutableHashSetSurrogate(ImmutableHashSet<T> immutable)
        {
            this.builder = immutable.ToBuilder();
        }

        public override object ToImmutable() => this.builder.ToImmutable();

        //-----------------------------------------------------------------------------------------

        public int Count => this.builder.Count;

        public bool IsReadOnly => false;

        //-----------------------------------------------------------------------------------------

        public bool Add(T item) => this.builder.Add(item);

        public void Clear() => this.builder.Clear();

        public bool Contains(T item) => this.builder.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();

        public void ExceptWith(IEnumerable<T> other) => this.builder.ExceptWith(other);

        public IEnumerator<T> GetEnumerator() => this.builder.GetEnumerator();

        public void IntersectWith(IEnumerable<T> other) => this.builder.IntersectWith(other);

        public bool IsProperSubsetOf(IEnumerable<T> other) => this.builder.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => this.builder.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other) => this.builder.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => this.builder.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other) => this.builder.Overlaps(other);

        public bool Remove(T item) => this.builder.Remove(item);

        public bool SetEquals(IEnumerable<T> other) => this.builder.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<T> other) => this.builder.SymmetricExceptWith(other);

        public void UnionWith(IEnumerable<T> other) => this.builder.UnionWith(other);

        void ICollection<T>.Add(T item) => this.builder.Add(item);

        IEnumerator IEnumerable.GetEnumerator() => this.builder.GetEnumerator();
    }
}
