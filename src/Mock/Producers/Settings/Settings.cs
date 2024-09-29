namespace Producers.Settings;

public static class Settings
{
    public static class Metadata
    {
        public static class Assets
        {
            public const string Path = "Metadatas/AssetMetadata.json";
        }
        
        public static class Briefing
        {
            public const string Path = "Metadatas/BriefingMetadata.json";
        }
        
        public static class ContentDistribution
        {
            public const string Path = "Metadatas/ContentDistributionMetadata.json";
        }
        
        public static class OrderList
        {
            public const string Path = "Metadatas/OrderListMetadata.json";
        }
    }
    
    public static class Topics
    {
        public static class Asset
        {
            public const string Name = "asset-metadata-topic";
        }
        
        public static class Briefing
        {
            public const string Name = "briefing-metadata-topic";
        }
        
        public static class ContentDistribution
        {
            public const string Name = "content-distribution-metadata-topic";
        }
        
        public static class OrderList
        {
            public const string Name = "order-list-metadata-topic";
        }
    }
}