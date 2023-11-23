using EndpointManager.Helper;
using EndpointManager.Model;
using EndpointManager.Service;
using Moq;

namespace EndpointManagerTest.Service
{
    public class EndpointServiceTest
    {
        private readonly Mock<ICacheService> _memoryCache = new Mock<ICacheService>();
        private readonly Mock<ILoggerService> _logger = new Mock<ILoggerService>();
        private readonly Mock<IUserInput> _userInput = new Mock<IUserInput>();

        [SetUp]
        public void SetUp()
        {
            _memoryCache.Reset();
        }

        public EndpointService ObterService()
        {
            return new EndpointService
            (
                _memoryCache.Object,
                _logger.Object,
                _userInput.Object
            );
        }

        [Order(1)]
        [Test]
        public void InsertNewEndpoint()
        {
            var endpoint = new EndpointModel
            {
                EndpointSerialNumber = "ABC12345",
                MeterModelId = 18,
                MeterNumber = 654321,
                MeterFirmwareVersion = "987ABC654",
                SwitchState = 0
            };
            _memoryCache.Setup(x => x.Set(endpoint.EndpointSerialNumber, endpoint));
            _userInput.SetupSequence(x => x.ReadLine())
                .Returns(endpoint.EndpointSerialNumber)
                .Returns("NSX1P2W")
                .Returns(endpoint.MeterNumber.ToString())
                .Returns(endpoint.MeterFirmwareVersion)
                .Returns("Connected");

            var service = ObterService();
            service.InsertNewEndpoint();

            _memoryCache.Verify(x => x.Set(endpoint.EndpointSerialNumber, It.IsAny<EndpointModel>()), Times.Once);
            _logger.Verify(x => x.LogInformation("Endpoint created!"), Times.Once);
            Assert.Pass();
        }

        [Order(2)]
        [Test]
        public void InsertNewEndpoint_DuplicatedSerialNumber()
        {
            var endpoint = new EndpointModel
            {
                EndpointSerialNumber = "ABC12345",
                MeterModelId = 18,
                MeterNumber = 654321,
                MeterFirmwareVersion = "987ABC654",
                SwitchState = 0
            };
            _memoryCache.Setup(x => x.TryGetValue(endpoint.EndpointSerialNumber, out endpoint))
                       .Returns(true);

            _userInput.SetupSequence(x => x.ReadLine())
                .Returns("ABC12345")
                .Returns("NSX3P4W")
                .Returns("11111")
                .Returns("123456789")
                .Returns("Disconnected");

            var service = ObterService();
            service.InsertNewEndpoint();

            _logger.Verify(x => x.LogError("Endpoint Serial Number already exists! Endpoint not created"), Times.Once);
            Assert.Pass();
        }

        [Order(3)]
        [Test]
        public void InsertNewEndpoint_InvalidMeterModel()
        {
            _userInput.SetupSequence(x => x.ReadLine())
                .Returns("ABC12345")
                .Returns("XX1X2X2")
                .Returns("654321")
                .Returns("987ABC654")
                .Returns("Connected");

            var service = ObterService();
            service.InsertNewEndpoint();

            _logger.Verify(x => x.LogWarning("Model not found!"), Times.Once);
            _memoryCache.Verify(x => x.Set(It.IsAny<String>(), It.IsAny<EndpointModel>()), Times.Never);
            Assert.Pass();
        }

        [Order(4)]
        [Test]
        public void EditEndpointSwtichState()
        {
            var endpoint = new EndpointModel
            {
                EndpointSerialNumber = "ABC12345",
                MeterModelId = 18,
                MeterNumber = 654321,
                MeterFirmwareVersion = "987ABC654",
                SwitchState = 0
            };
            _userInput.Setup(x => x.ReadLine()).Returns("Connected");
            _memoryCache.Setup(x => x.TryGetValue(endpoint.EndpointSerialNumber, out endpoint))
                       .Returns(true);

            var service = ObterService();
            service.EditEndpointSwtichState(endpoint.EndpointSerialNumber);

            _logger.Verify(x => x.LogInformation("Switch state successfully changed!"), Times.Once);
            Assert.Pass();
        }

        [Order(5)]
        [Test]
        public void FindEndpointBySerialNumber()
        {
            var endpoint = new EndpointModel
            {
                EndpointSerialNumber = "ABC12345",
                MeterModelId = 18,
                MeterNumber = 654321,
                MeterFirmwareVersion = "987ABC654",
                SwitchState = 0
            };
            _memoryCache.Setup(x => x.TryGetValue(endpoint.EndpointSerialNumber, out endpoint))
                       .Returns(true);

            var service = ObterService();

            var result = service.FindEndpointBySerialNumber("ABC12345");

            Assert.That(result, Is.EqualTo(endpoint));
            Assert.Pass();
        }

        [Order(6)]
        [Test]
        public void FindEndpointBySerialNumber_NotFound()
        {
            var service = ObterService();
            var result = service.FindEndpointBySerialNumber("ABC987654");

            Assert.That(result, Is.Null);
            _logger.Verify(x => x.LogWarning("Endpoint Serial Number not found!"), Times.Once);
            Assert.Pass();
        }

        [Order(7)]
        [Test]
        public void DeleteEndpoint()
        {
            var endpoint = new EndpointModel
            {
                EndpointSerialNumber = "ABC12345",
                MeterModelId = 18,
                MeterNumber = 654321,
                MeterFirmwareVersion = "987ABC654",
                SwitchState = 0
            };

            _userInput.Setup(x => x.ReadLine()).Returns("1");
            _memoryCache.Setup(x => x.TryGetValue(endpoint.EndpointSerialNumber, out endpoint))
                       .Returns(true);
            _memoryCache.Setup(x => x.Remove(endpoint.EndpointSerialNumber));

            var service = ObterService();
            service.DeleteEndpoint(endpoint.EndpointSerialNumber);

            _memoryCache.Verify(x => x.Remove(endpoint.EndpointSerialNumber), Times.Once);
            _logger.Verify(logger => logger.LogInformation("Endpoint ABC12345 successfully deleted!"), Times.Once);
            Assert.Pass();
        }
    }

    public class MockUserInput : IUserInput
    {
        private readonly string _input;

        public MockUserInput(string input)
        {
            _input = input;
        }

        public string ReadLine()
        {
            return _input;
        }
    }
}
