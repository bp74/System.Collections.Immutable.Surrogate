# System.Collections.Immutable.Surrogate

A SerializationSurrogateProvider for the System.Collection.Immutable types

        [DataContract]
        class Foo
        {
            [DataMember]
            public ImmutableList<int> List { get; set; } = ImmutableList<int>.Empty;
        }
        
        var foo = new Foo();
        var fooSerialized = default(byte[]);
        
        foo.List = foo.List.Add(1);
        foo.List = foo.List.Add(2);
        foo.List = foo.List.Add(3);

        using (var memoryStream = new MemoryStream())
        {
            var serializer = new DataContractSerializer(typeof(Foo));
            serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());
            serializer.WriteObject(memoryStream, foo);
            fooSerialized = memoryStream.ToArray();
        }

        using (var memoryStream = new MemoryStream(fooSerialized))
        {
            var serializer = new DataContractSerializer(typeof(Foo));
            serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());
            foo = (Foo)serializer.ReadObject(memoryStream);
        }
