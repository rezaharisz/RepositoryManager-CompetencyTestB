using Repository;

class Program
{
    static void Main()
    {
        var repo = new RepositoryManager();
        repo.Initialize();

        repo.Register("json1", "{\"name\":\"test\"}", 1);
        repo.Register("xml1", "<root>xml test</root>", 2);

        Console.WriteLine(repo.Retrieve("json1"));
        Console.WriteLine(repo.GetType("json1"));

        Console.WriteLine(repo.Retrieve("xml1"));
        Console.WriteLine(repo.GetType("xml1"));

        repo.Deregister("json1");
        repo.Deregister("xml1");
    }
}