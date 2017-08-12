using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Immutable
{
    public class ImmutableDictionarySurrogate<TKey, TValue> : ImmutableSurrogate, IDictionary<TKey, TValue>
    {
        private ImmutableDictionary<TKey, TValue>.Builder builder;

        public ImmutableDictionarySurrogate()
        {
            this.builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
        }

        public ImmutableDictionarySurrogate(ImmutableDictionary<TKey, TValue> immutable)
        {
            this.builder = immutable.ToBuilder();
        }

        public override object ToImmutable() => this.builder.ToImmutable();

        //-----------------------------------------------------------------------------------------

        public TValue this[TKey key]
        {
            get => this.builder[key];
            set => this.builder[key] = value;
        }

        public ICollection<TKey> Keys => this.builder.Keys.ToArray();

        public ICollection<TValue> Values => this.builder.Values.ToArray();

        public int Count => this.builder.Count;

        public bool IsReadOnly => false;

        //-----------------------------------------------------------------------------------------

        public void Add(TKey key, TValue value) => this.builder.Add(key, value);

        public void Add(KeyValuePair<TKey, TValue> item) => this.builder.Add(item);

        public bool Remove(TKey key) => this.builder.Remove(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => this.builder.Remove(item);

        public void Clear() => this.builder.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => this.builder.Contains(item);

        public bool ContainsKey(TKey key) => this.builder.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => this.builder.TryGetValue(key, out value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new NotSupportedException();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.builder.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.builder.GetEnumerator();
    }
}
