using Clickatell.Services.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clickatell.Services.Data;

namespace Clickatell.Services.Tests
{
    [TestClass]
    public class HTPPTestFixtures
    {
        private readonly APIClient _apiClient;
        public HTPPTestFixtures()
        {
            _apiClient = new HTTP(new HTTPCredentials("userName", "password", "apiId"));
        }

        [TestMethod]
        public void Authenticate()
        {
            var result = _apiClient.Authenticate();
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void SendMessage()
        {
            var result = _apiClient.SendMessage(new SendMessageRequest("Message Text", "MSISDN"));
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetBalance()
        {
            var result = _apiClient.GetBalance();
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void StopMessage()
        {
            var result = _apiClient.StopMessage(new APIMessageRequest("MessageAPIId"));
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetCoverage()
        {
            var result = _apiClient.GetCoverage(new MessageRequest("MSISDN"));
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetMessageCharge()
        {
            var result = _apiClient.GetMessageCharge(new APIMessageRequest("MessageAPIId"));
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetMessageStatus()
        {
            var result = _apiClient.GetMessageStatus(new APIMessageRequest("MessageAPIId"));
            Assert.IsTrue(result.Success);
        }
    }
}
