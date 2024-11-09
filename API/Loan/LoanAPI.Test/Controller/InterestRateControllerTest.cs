using BrassLoon.Interface.Config;
using BrassLoon.Interface.Config.Models;
using JestersCreditUnion.Interface.Loan.Models;
using LoanAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Dynamic;
using System.Threading.Tasks;
using AuthorizationAPI = BrassLoon.Interface.Authorization;

namespace LoanAPI.Test.Controller
{
    [TestClass]
    public class InterestRateControllerTest
    {
        private static readonly Guid _configurationDomainId = Guid.NewGuid();
        private static readonly string _itemCode = "ut-item-code";

        private static InterestRateController CreateController(
            Settings settings = null,
            ISettingsFactory settingsFactory = null,
            AuthorizationAPI.IUserService userService = null,
            ILogger<InterestRateController> logger = null,
            IItemService itemService = null)
        {
            Mock<IOptions<Settings>> options = new Mock<IOptions<Settings>>();
            options.SetupGet(o => o.Value).Returns(settings ?? CreateSettings());
            if (settingsFactory == null)
                settingsFactory = new Mock<ISettingsFactory>().Object;
            if (userService == null)
                userService = new Mock<AuthorizationAPI.IUserService>().Object;
            if (logger == null)
                logger = new Mock<ILogger<InterestRateController>>().Object;
            if (itemService == null)
                itemService = new Mock<IItemService>().Object;
            return new InterestRateController(
                options.Object,
                settingsFactory,
                userService,
                logger,
                itemService);
        }

        private static Settings CreateSettings()
        {
            return new Settings
            {
                AuthorizationDomainId = Guid.Empty,
                ConfigDomainId = _configurationDomainId,
                InterestRateConfigurationCode = _itemCode
            };
        }

        private static async Task<IActionResult> GetTest(
            Mock<IItemService> itemService = null)
        {
            InterestRateController controller = CreateController(itemService: itemService?.Object);
            IActionResult result = await controller.Get();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<OkObjectResult>(result);
            return result;
        }

        [TestMethod]
        public async Task GetNewTest()
        {
            Mock<IItemService> itemService = new Mock<IItemService>();
            IActionResult result = await GetTest(itemService);
            itemService.Verify(s => s.GetByCode(It.IsAny<ISettings>(), _configurationDomainId, _itemCode), Times.Once);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType<InterestRateConfiguration>(okObjectResult.Value);
            InterestRateConfiguration interestRateConfiguration = (InterestRateConfiguration)okObjectResult.Value;
            Assert.IsTrue(interestRateConfiguration.InflationRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.InflationRate.Value);
            Assert.IsTrue(interestRateConfiguration.OperationsRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.OperationsRate.Value);
            Assert.IsTrue(interestRateConfiguration.LossRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.LossRate.Value);
            Assert.IsTrue(interestRateConfiguration.IncentiveRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.IncentiveRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.OtherRate.Value);
            Assert.IsTrue(interestRateConfiguration.TotalRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.TotalRate.Value);
            Assert.IsTrue(interestRateConfiguration.MinimumRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.MinimumRate.Value);
            Assert.IsTrue(interestRateConfiguration.MaximumRate.HasValue);
            Assert.AreEqual(0.0M, interestRateConfiguration.MaximumRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRateDescription != null);
        }

        [TestMethod]
        public async Task GetExpandoTest()
        {
            dynamic config = new ExpandoObject();
            config.InflationRate = 11.11M;
            config.OperationsRate = 22.22M;
            config.LossRate = 33.33M;
            config.IncentiveRate = 44.44M;
            config.OtherRate = 55.55M;
            config.TotalRate = 66.66M;
            config.MinimumRate = 77.77M;
            config.MaximumRate = 88.88M;
            config.OtherRateDescription = "test description";
            Mock<IItemService> itemService = new Mock<IItemService>();
            itemService.Setup(s => s.GetByCode(It.IsAny<ISettings>(), _configurationDomainId, _itemCode))
                .Returns(() => Task.FromResult(new Item { Data = config }));
            IActionResult result = await GetTest(itemService);
            itemService.Verify(s => s.GetByCode(It.IsAny<ISettings>(), _configurationDomainId, _itemCode), Times.Once);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType<InterestRateConfiguration>(okObjectResult.Value);
            InterestRateConfiguration interestRateConfiguration = (InterestRateConfiguration)okObjectResult.Value;
            Assert.IsTrue(interestRateConfiguration.InflationRate.HasValue);
            Assert.AreEqual(11.11M, interestRateConfiguration.InflationRate.Value);
            Assert.IsTrue(interestRateConfiguration.OperationsRate.HasValue);
            Assert.AreEqual(22.22M, interestRateConfiguration.OperationsRate.Value);
            Assert.IsTrue(interestRateConfiguration.LossRate.HasValue);
            Assert.AreEqual(33.33M, interestRateConfiguration.LossRate.Value);
            Assert.IsTrue(interestRateConfiguration.IncentiveRate.HasValue);
            Assert.AreEqual(44.44M, interestRateConfiguration.IncentiveRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRate.HasValue);
            Assert.AreEqual(55.55M, interestRateConfiguration.OtherRate.Value);
            Assert.IsTrue(interestRateConfiguration.TotalRate.HasValue);
            Assert.AreEqual(66.66M, interestRateConfiguration.TotalRate.Value);
            Assert.IsTrue(interestRateConfiguration.MinimumRate.HasValue);
            Assert.AreEqual(77.77M, interestRateConfiguration.MinimumRate.Value);
            Assert.IsTrue(interestRateConfiguration.MaximumRate.HasValue);
            Assert.AreEqual(88.88M, interestRateConfiguration.MaximumRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRateDescription != null);
            Assert.AreEqual("test description", interestRateConfiguration.OtherRateDescription);
        }

        [TestMethod]
        public async Task GetNewtonsoftTest()
        {
            InterestRateConfiguration config = new InterestRateConfiguration
            {
                InflationRate = 11.11M,
                OperationsRate = 22.22M,
                LossRate = 33.33M,
                IncentiveRate = 44.44M,
                OtherRate = 55.55M,
                TotalRate = 66.66M,
                MinimumRate = 77.77M,
                MaximumRate = 88.88M,
                OtherRateDescription = "test description"
            };
            Mock<IItemService> itemService = new Mock<IItemService>();
            itemService.Setup(s => s.GetByCode(It.IsAny<ISettings>(), _configurationDomainId, _itemCode))
                .Returns(() =>
                {
                    string json = JsonConvert.SerializeObject(config, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
                    return Task.FromResult(new Item { Data = JsonConvert.DeserializeObject(json) });
                });
            IActionResult result = await GetTest(itemService);
            itemService.Verify(s => s.GetByCode(It.IsAny<ISettings>(), _configurationDomainId, _itemCode), Times.Once);
            OkObjectResult okObjectResult = (OkObjectResult)result;
            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType<InterestRateConfiguration>(okObjectResult.Value);
            InterestRateConfiguration interestRateConfiguration = (InterestRateConfiguration)okObjectResult.Value;
            Assert.IsTrue(interestRateConfiguration.InflationRate.HasValue);
            Assert.AreEqual(11.11M, interestRateConfiguration.InflationRate.Value);
            Assert.IsTrue(interestRateConfiguration.OperationsRate.HasValue);
            Assert.AreEqual(22.22M, interestRateConfiguration.OperationsRate.Value);
            Assert.IsTrue(interestRateConfiguration.LossRate.HasValue);
            Assert.AreEqual(33.33M, interestRateConfiguration.LossRate.Value);
            Assert.IsTrue(interestRateConfiguration.IncentiveRate.HasValue);
            Assert.AreEqual(44.44M, interestRateConfiguration.IncentiveRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRate.HasValue);
            Assert.AreEqual(55.55M, interestRateConfiguration.OtherRate.Value);
            Assert.IsTrue(interestRateConfiguration.TotalRate.HasValue);
            Assert.AreEqual(66.66M, interestRateConfiguration.TotalRate.Value);
            Assert.IsTrue(interestRateConfiguration.MinimumRate.HasValue);
            Assert.AreEqual(77.77M, interestRateConfiguration.MinimumRate.Value);
            Assert.IsTrue(interestRateConfiguration.MaximumRate.HasValue);
            Assert.AreEqual(88.88M, interestRateConfiguration.MaximumRate.Value);
            Assert.IsTrue(interestRateConfiguration.OtherRateDescription != null);
            Assert.AreEqual("test description", interestRateConfiguration.OtherRateDescription);
        }
    }
}
