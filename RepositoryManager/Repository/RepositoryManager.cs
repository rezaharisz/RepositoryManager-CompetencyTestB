using Enum;
using System.Collections.Concurrent;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ConcurrentDictionary<string, RepositoryItem> _storage = new();
        private bool _initialized = false;
        private readonly object _initLock = new();

        public void Initialize()
        {
            lock (_initLock)
            {
                if (_initialized) return;
                _initialized = true;
            }
        }

        public void Register(string itemName, string itemContent, int itemType)
        {
            CheckInitialization();

            if (_storage.ContainsKey(itemName))
            {
                throw new InvalidOperationException($"Item '{itemName}' already registered.");
            }

            if (!ValidateContent(itemContent, itemType))
            {
                throw new ArgumentException("Invalid content format.");
            }

            var type = (ItemType) itemType;
            var item = new RepositoryItem(itemContent, type);
            if (!_storage.TryAdd(itemName, item))
            {
                throw new Exception("Unable to add item due to internal error.");
            }
        }

        public string Retrieve(string itemName)
        {
            CheckInitialization();

            if (_storage.TryGetValue(itemName, out var item))
            {
                return item.Content;
            }

            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }

        public int GetType(string itemName)
        {
            CheckInitialization();

            if (_storage.TryGetValue(itemName, out var item))
            {
                return (int)item.Type;
            }

            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }

        public void Deregister(string itemName)
        {
            CheckInitialization();

            if (!_storage.TryRemove(itemName, out _))
            {
                throw new KeyNotFoundException($"Item '{itemName}' not found.");
            }
        }

        private bool ValidateContent(string content, int type)
        {
            if (type == (int)ItemType.Json)
            {
                return content.TrimStart().StartsWith("{") && content.TrimEnd().EndsWith("}");
            }
            else if (type == (int)ItemType.Xml)
            {
                return content.TrimStart().StartsWith("<") && content.TrimEnd().EndsWith(">");
            }

            return false;
        }

        private void CheckInitialization()
        {
            if (!_initialized)
                throw new InvalidOperationException("Repository must be initialized before use.");
        }
    }
}
