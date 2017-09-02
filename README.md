# System.Collections.Immutable.Surrogate

A SerializationSurrogateProvider for the System.Collection.Immutable types.

Also available as Nuget package:
<https://www.nuget.org/packages/System.Collections.Immutable.Surrogate/>

        [DataContract]
        class Foo
        {
            [DataMember]
            public ImmutableList<int> List { get; set; } = ImmutableList<int>.Empty;
        }
        
        var foo = new Foo();
        var fooSerialized = default(byte[]);
        
        foo.List = foo.List.Add(1).Add(2).Add(3);

        // .Net Standard 2.0
        var serializer = new DataContractSerializer(typeof(Foo));
        serializer.SetSerializationSurrogateProvider(new ImmutableSurrogateProvider());

        // .Net Framework 4.5
        // var settings = new DataContractSerializerSettings();
        // settings.DataContractSurrogate = new ImmutableSurrogateProvider();
        // var serializer = new DataContractSerializer(typeof(T), settings);

        using (var memoryStream = new MemoryStream())
        {
            serializer.WriteObject(memoryStream, foo);
            fooSerialized = memoryStream.ToArray();
        }

        using (var memoryStream = new MemoryStream(fooSerialized))
        {
            foo = (Foo)serializer.ReadObject(memoryStream);
        }
