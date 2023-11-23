namespace EndpointManager.Helper
{
    public interface ICacheService
    {
        void Remove(string key);
        void Set<Item>(string key, Item value);
        bool TryGetValue<Item>(string key, out Item value);
    }
}