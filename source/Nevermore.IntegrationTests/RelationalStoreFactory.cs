using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nevermore.IntegrationTests
{
    public class RelationalStoreFactory : IRelationalStoreFactory
    {
        readonly string connectionString;
        private readonly string applicationName;
        private readonly JsonSerializerSettings jsonSettings;
        private readonly IContractResolver contractResolver;
        private readonly IEnumerable<JsonConverter> converters;
        private readonly KeyAllocator keyAllocator;
        readonly Lazy<RelationalStore> relationalStore;

        public RelationalStoreFactory(string connectionString, string applicationName, IContractResolver contractResolver = null, JsonSerializerSettings jsonSettings = null, IEnumerable<JsonConverter> converters = null, KeyAllocator keyAllocator = null)
        {
            this.connectionString = connectionString;
            this.applicationName = applicationName;
            this.jsonSettings = jsonSettings;
            this.contractResolver = contractResolver ?? new DefaultContractResolver();
            this.converters = converters ?? new List<JsonConverter>();
            this.keyAllocator = keyAllocator;

            relationalStore = new Lazy<RelationalStore>(InitializeRelationalStore);
        }

        public RelationalStore RelationalStore
        {
            get { return relationalStore.Value; }
        }

        public static RelationalMappings CreateMappings()
        {
            var mappings = new RelationalMappings();

            var mappers = new List<DocumentMap>();
            mappings.Install(mappers);

            return mappings;
        }

        RelationalStore InitializeRelationalStore()
        {
            return new RelationalStore(connectionString, applicationName, CreateMappings(), contractResolver, converters, jsonSettings, keyAllocator);
        }
    }
}