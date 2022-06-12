using Microsoft.Extensions.Configuration;
using ZeroCodingTask.ConfigurationStorage.Extentions;

namespace ConfigurationUnitTests
{
    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void TestWritingConfigurations()
        {
            string testConfigFile = "TestConfigFile.txt";
            try
            {
                // Arrange
                var testKey = "TestKey";
                var testValue = "TestValue";

                File.WriteAllText(testConfigFile, testKey + ":" + testValue + Environment.NewLine);

                // Act
                var config = new ConfigurationBuilder().AddFileStorageConfiguration(testConfigFile, reloadOnChange: false).Build();
                var actualValue = config[testKey];

                // Assert
                Assert.AreEqual(testValue, actualValue, "Writing configuration failed");
            }
            finally
            {
                File.Delete(testConfigFile);
            }
        }

        [TestMethod]
        public void TestNestedWritingConfigurations()
        {
            string testConfigFile1 = "TestConfigFile1.txt";
            string testConfigFile2 = "TestConfigFile2.txt";
            try
            {
                // Arrange
                var testKey1 = "TestKey1";
                var testValue1 = "TestValue1";

                var testKey2 = "TestKey2";
                var testValue2 = "TestValue2";

                var nodeName = "testConfig2";

                var list = new[] { testKey1 + ":" + testValue1, $"FILE:{nodeName}:{testConfigFile2}" };
                File.WriteAllLines(testConfigFile1, list);

                File.WriteAllText(testConfigFile2, testKey2 + ":" + testValue2 + Environment.NewLine);

                // Act
                var config = new ConfigurationBuilder().AddFileStorageConfiguration(testConfigFile1, reloadOnChange: false).Build();
                var actualValue = config[nodeName + ":" + testKey2];

                // Assert
                Assert.AreEqual(testValue2, actualValue, "Writing configuration failed");
            }
            finally
            {
                File.Delete(testConfigFile1);
                File.Delete(testConfigFile2);
            }
        }

        [TestMethod]
        public void TestReadingConfigurations()
        {
            // Arrange
            var expectedValue = true;
            var value1 = "321";
            var value2 = "keyBar value";
            var value3 = "true";

            var config = new ConfigurationBuilder().AddFileStorageConfiguration("Configs/root.txt", reloadOnChange: false).Build();

            config.GetSection("nodeFoo")["key123"] = value1;
            config["keyBar"] = value2;
            config.GetSection("nodeBar").GetSection("innernode2")["key888"] = value3;


            // Act
            var result1 = config.GetSection("nodeFoo")["key123"] == value1;
            var result2 = config["keyBar"] == value2;
            var result3 = config.GetSection("nodeBar").GetSection("innernode2")["key888"] == value3;

            // Assert
            Assert.AreEqual(expectedValue, result1, "Reading configuration failed");
            Assert.AreEqual(expectedValue, result2, "Reading configuration failed");
            Assert.AreEqual(expectedValue, result3, "Reading configuration failed");
        }


        [TestMethod]
        public void TestReloadingConfigurations()
        {
            string testConfigFile = "TestConfigFile.txt";
            try
            {
                // Arrange
                var testKey = "TestKey";
                var testValue = "TestValue";

                File.WriteAllText(testConfigFile, "key" + ":" + "value" + Environment.NewLine);

                var config = new ConfigurationBuilder().AddFileStorageConfiguration(testConfigFile, reloadOnChange: true).Build();
                var lineToAppend = testKey + ":" + testValue + Environment.NewLine;

                File.AppendAllLines(testConfigFile, new List<string> { lineToAppend });

                Thread.Sleep(5000);

                Assert.AreEqual(testValue, config[testKey], "Reloading failed");
            }
            finally
            {
                File.Delete(testConfigFile);
            }
        }
    }
}