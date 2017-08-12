using System.Collections.Generic;

namespace System.Collections.Immutable
{
    public class ImmutableArraySurrogate<T> : ImmutableSurrogate, ICollection<T>
    {
        private ImmutableArray<T>.Builder builder;

        public ImmutableArraySurrogate()
        {
            this.builder = ImmutableArray.CreateBuilder<T>();
        }

        public ImmutableArraySurrogate(ImmutableArray<T> immutable)
        {
            this.builder = immutable.ToBuilder();
        }

        public override object ToImmutable() => this.builder.ToImmutable();

        //-----------------------------------------------------------------------------------------

        public T this[int index]
        {
            get => this.builder[index];
            set => this.builder[index] = value;
        }

        public int Count => this.builder.Count;

        public bool IsReadOnly => false;

        //-----------------------------------------------------------------------------------------

        public void Add(T item) => this.builder.Add(item);

        public void Clear() => this.builder.Clear();

        public bool Contains(T item) => this.builder.Contains(item);

        public int IndexOf(T item) => this.builder.IndexOf(item);

        public void Insert(int index, T item) => this.builder.Insert(index, item);

        public bool Remove(T item) => this.builder.Remove(item);

        public void RemoveAt(int index) => this.builder.RemoveAt(index);

        public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => this.builder.GetEnumerator();
         
        IEnumerator IEnumerable.GetEnumerator() => this.builder.GetEnumerator();
    }
}
