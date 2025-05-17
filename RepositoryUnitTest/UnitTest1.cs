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
        // Scenario :
        // 1. Initialize Repository
        // 2. Initialize the Repository again
        // 3. Expected to not return any error
        [Fact]
        public void Initialize_CanBeCalledMultipleTimesWithoutError()
        {
            var repo = new RepositoryManager();
            repo.Initialize();
            repo.Initialize();
        }

        // Verify that able to Register valid json
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "item1" with value "{\"name\":\"test\"}" and item type 1 (Json)
        // 3. Check result is it equal to value "{\"name\":\"test\"}" or not
        // 4. Expected result to have same value as "{\"name\":\"test\"}"
        [Fact]
        public void Register_ValidJson_AddsSuccessfully()
        {
            var repo = CreateInitializedRepo();
            repo.Register("item1", "{\"name\":\"test\"}", 1);
            var result = repo.Retrieve("item1");
            Assert.Equal("{\"name\":\"test\"}", result);
        }

        // Verify that able to Register valid xml
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "item2" with value "<root>xmlTest</root>" and item type 2 (XML)
        // 3. Check result is it equal to value "<root>xmlTest</root>" or not
        // 4. Expected result to have same value as ("<root>xmlTest</root>"
        [Fact]
        public void Register_ValidXml_AddsSuccessfully()
        {
            var repo = CreateInitializedRepo();
            repo.Register("item2", "<root>xmlTest</root>", 2);
            var result = repo.Retrieve("item2");
            Assert.Equal("<root>xmlTest</root>", result);
        }

        // Verify that Register invalid json will throw exception
        // Scenario :
        // 1. Initialize Repository
        // 2. Try Register "badJson" with value "invalid json" and item type 1 (JSON)
        // 3. Expected to throw exception
        [Fact]
        public void Register_InvalidJson_ThrowsArgumentException()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<ArgumentException>(() =>
                repo.Register("badJson", "invalid json", 1));
        }

        // Verify that Register invalid xml will throw exception
        // Scenario :
        // 1. Initialize Repository
        // 2. Try Register "badXml" with value "not xml" and item type 2 (XML)
        // 3. Expected to throw exception
        [Fact]
        public void Register_InvalidXml_ThrowsArgumentException()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<ArgumentException>(() =>
                repo.Register("badXml", "not xml", 2));
        }

        // Verify that Register duplicate item will throw exception
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "dupItem" with value "{\"a\":1}" and item type 1 (JSON)
        // 3. Register "dupItem" again with value "{\"a\":2}" and item type 1 (JSON)
        // 4. Expected to throw exception
        [Fact]
        public void Register_DuplicateItem_ThrowsInvalidOperationException()
        {
            var repo = CreateInitializedRepo();
            repo.Register("dupItem", "{\"a\":1}", 1);

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("dupItem", "{\"a\":2}", 1));
        }

        // Verify that Retrieve return correct content from Register
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "jsonItem" with value "{\"x\":1}" and item type 1 (JSON)
        // 3. Retrieve "jsonItem" value
        // 4. Check is it the Retrieve value equals to value that we Register before, which is "{\"x\":1}"
        // 5. Expected result to have same value as "{\"x\":1}"
        [Fact]
        public void Retrieve_ExistingItem_ReturnsCorrectContent()
        {
            var repo = CreateInitializedRepo();
            repo.Register("jsonItem", "{\"x\":1}", 1);
            string result = repo.Retrieve("jsonItem");

            Assert.Equal("{\"x\":1}", result);
        }

        // Verify that Retrieve will throw exception when try to Retrieve non existing item
        // Scenario :
        // 1. Initialize Repository
        // 2. Try to Retrieve "notExists"
        // 3. Expected to throw exception
        [Fact]
        public void Retrieve_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.Retrieve("notExists"));
        }

        // Verify that GetType return correct type of the item
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "typeTest" with value "<node></node>" and item type 2 (XML)
        // 3. Call GetType with parameter "typeTest"
        // 4. Check result is it return item type 2 (XML) or not
        // 5. Expected result to return same item type value, which is 2 (XML)
        [Fact]
        public void GetType_ExistingItem_ReturnsCorrectType()
        {
            var repo = CreateInitializedRepo();
            repo.Register("typeTest", "<node></node>", 2);

            int result = repo.GetType("typeTest");
            Assert.Equal(2, result);
        }

        // Verify that GetType will throw exception when try to GetType with non existing Key
        // Scenario :
        // 1. Initialize Repository
        // 2. Try to GetType with key "notExists"
        // 3. Expected to throw exception
        [Fact]
        public void GetType_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.GetType("notExists"));
        }

        // Verify that able to Deregister existing item
        // Scenario :
        // 1. Initialize Repository
        // 2. Register "deleteItem" with value "{\"item\": 0}" and item type 1 (JSON)
        // 3. Deregister "deleteItem"
        // 4. Try to Retrieve "deleteItem"
        // 5. Expected to throw exception
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
        // Scenario :
        // 1. Initialize Repository
        // 2. Try to Deregister "notExists"
        // 3. Expected to throw exception
        [Fact]
        public void Deregister_NonExistentItem_ThrowsKeyNotFound()
        {
            var repo = CreateInitializedRepo();
            Assert.Throws<KeyNotFoundException>(() =>
                repo.Deregister("notExists"));
        }

        // Verify that when try to call method without initialize the Repository will throw an exception
        // Scenario :
        // 1. Initialize Repository
        // 2. Try to Register "test" with value "{\"a\":1}" and item type 1 (JSON)
        // 3. Expected to throw exception
        // 4. Try to Retrieve "test"
        // 4. Expected to throw exception
        // 5. Try to GetType "test"
        // 6. Expected to throw exception
        // 7. Try to Deregister "test"
        // 8. Expected to throw exception
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