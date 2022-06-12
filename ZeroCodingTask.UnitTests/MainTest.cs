using Microsoft.Extensions.Configuration;
using System.Text;
using ZeroCodingTask.ConfigurationStorage.Extentions;

namespace ConfigurationUnitTests
{
    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void TestWritingConfigurations()
        {
            // Arrange
            string testConfigFile = "TestConfigFile.txt";
            var testKey = "TestKey";
            var testValue = "TestValue";
            if (File.Exists(testConfigFile))
            {
                File.Delete(testConfigFile);
            }

            using (FileStream fs = File.Create(testConfigFile))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(testKey + ":" + testValue);
                fs.Write(title, 0, title.Length);
            }

            // Act
            var config = new ConfigurationBuilder().AddFileStorageConfiguration("TestConfigFile.txt", reloadOnChange: false).Build();
            var actualValue = config[testKey];

            // Assert
            var expectedValue = testValue;

            Assert.AreEqual(expectedValue, actualValue, "Writing configuration failed");
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
            // Arrange
            var newKey = "newKey";
            var newValue = "newValue";
            var config = new ConfigurationBuilder().AddFileStorageConfiguration("Configs/root.txt", reloadOnChange: true).Build();
            var lineToAppend = newKey + ":" + newValue + Environment.NewLine;

            File.AppendAllLines("Configs/root.txt", new List<string> { lineToAppend });

            Thread.Sleep(1000);

            Assert.AreEqual(newValue, config[newKey], "Reloading failed");
        }
    }
}