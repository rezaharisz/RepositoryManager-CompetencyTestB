namespace Repository
{
    public interface IRepositoryManager
    {
        void Initialize();
        void Register(string itemName, string itemContent, int itemType);
        string Retrieve(string itemName);
        int GetType(string itemName);
        void Deregister(string itemName);
    }
}
