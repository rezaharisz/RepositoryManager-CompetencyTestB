using Repository;

namespace RepositoryUnitTest
{
    public class UnitTest1
    {
        private RepositoryManager CreateInitializedRepo()
        {
            var repo = new RepositoryManager();
            repo.Initialize();
            return repo;
        }

        // Verify that able to initialize Repository by multiple time without error
        [Fact]
        public void Initialize_CanBeCalledMultipleTimesWithoutError()
        {
            var repo = new RepositoryManager();
            repo.Initialize();
            repo.Initialize();
        }

        // Verify that able to Register valid json
        [Fact]
        public void Register_ValidJson_AddsSuccessfully()
        {
            var repo = CreateInitializedRepo();
            repo.Register("item1", "{\"name\":\"test\"}", 1);
            var result = repo.Retrieve("item1");
            Assert.Equal("{\"name\":\"test\"}", result);
        }

        // Verify that able to Register valid xml
        [Fact]
        public void Register_ValidXml_AddsSuccessfully()
        {
            var repo = CreateInitializedRepo();
            repo.Register("item2", "<root>xmlTest</root>", 2);
            var result = repo.Retrieve("item2");
            Assert.Equal("<root>xmlTest</root>", result);
        }

        // Verify that Register invalid json will throw exception
        [Fact]
        public void Register_InvalidJson_ThrowsArgumentException()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<ArgumentException>(() =>
                repo.Register("badJson", "invalid json", 1));
        }

        // Verify that Register invalid xml will throw exception
        [Fact]
        public void Register_InvalidXml_ThrowsArgumentException()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<ArgumentException>(() =>
                repo.Register("badXml", "not xml", 2));
        }

        // Verify that Register duplicate item will throw exception
        [Fact]
        public void Register_DuplicateItem_ThrowsInvalidOperationException()
        {
            var repo = CreateInitializedRepo();
            repo.Register("dupItem", "{\"a\":1}", 1);

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("dupItem", "{\"a\":2}", 1));
        }

        // Verify that Retrieve return correct content from Register
        [Fact]
        public void Retrieve_ExistingItem_ReturnsCorrectContent()
        {
            var repo = CreateInitializedRepo();
            repo.Register("jsonItem", "{\"x\":1}", 1);
            string result = repo.Retrieve("jsonItem");

            Assert.Equal("{\"x\":1}", result);
        }

        // Verify that Retrieve will throw exception when try to Retrieve non existing item
        [Fact]
        public void Retrieve_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.Retrieve("notExists"));
        }

        // Verify that GetType return correct type of the item
        [Fact]
        public void GetType_ExistingItem_ReturnsCorrectType()
        {
            var repo = CreateInitializedRepo();
            repo.Register("typeTest", "<node></node>", 2);

            int result = repo.GetType("typeTest");
            Assert.Equal(2, result);
        }

        // Verify that GetType will throw exception when try to GetType with non existing item
        [Fact]
        public void GetType_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.GetType("notExists"));
        }

        // Verify that able to Deregister existing item
        [Fact]
        public void Deregister_ExistingItem_RemovesSuccessfully()
        {
            var repo = CreateInitializedRepo();
            repo.Register("deleteItem", "{\"item\": 0}", 1);
            repo.Deregister("deleteItem");

            Assert.Throws<KeyNotFoundException>(() =>
                repo.Retrieve("deleteItem"));
        }

        // Verify that Deregister non existing item will throw exception
        [Fact]
        public void Deregister_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.Deregister("notExists"));
        }

        // Verify that when try to call method without initialize the Repository will throw an exception
        [Fact]
        public void Methods_WithoutInitialize_ThrowInvalidOperationException()
        {
            var repo = new RepositoryManager();

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("test", "{\"a\":1}", 1));

            Assert.Throws<InvalidOperationException>(() =>
                repo.Retrieve("test"));

            Assert.Throws<InvalidOperationException>(() =>
                repo.GetType("test"));

            Assert.Throws<InvalidOperationException>(() =>
                repo.Deregister("test"));
        }

    }
}