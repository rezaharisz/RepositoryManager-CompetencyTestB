using Enum;

namespace Repository
{
    public class RepositoryItem
    {
        public string Content { get; }
        public ItemType
            Type { get; }

        public RepositoryItem(string content, ItemType type)
        {
            Content = content;
            Type = type;
        }
    }
}
