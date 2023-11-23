using EndpointManager.Helper;
using EndpointManager.Model;

namespace EndpointManager.Service
{
    public class EndpointService : IEndpointService
    {
        private readonly ICacheService _memoryCache;
        private readonly ILoggerService _logger;
        private readonly IUserInput _userInput;

        public EndpointService(ICacheService memoryCache, ILoggerService logger, IUserInput userInput)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _userInput = userInput;
        }

        private List<string> SerialNumberList = new();

        public void InsertNewEndpoint()
        {
            try
            {
                EndpointModel endpoint = new();

                Console.WriteLine("Insert a Endpoint Serial Number:");
                string userInput = _userInput.ReadLine();

                if (userInput.Length == 0)
                {
                    _logger.LogWarning("The Endpoint Serial Number can't be null, try again!");
                    return;
                }

                endpoint.EndpointSerialNumber = userInput;

                Console.WriteLine("Insert the Meter Model Id:");
                var ModelId = endpoint.MeterModelId = EndpointModel.SetMeterModelId(_userInput.ReadLine());

                if (ModelId == -1)
                {
                    _logger.LogWarning("Model not found!");
                    return;
                }

                Console.WriteLine("Insert the Meter Number:");
                if (!int.TryParse(_userInput.ReadLine(), out int MeterNumber))
                {
                    _logger.LogWarning("Please insert a valid Meter Number, try again!");
                    return;
                }

                endpoint.MeterNumber = MeterNumber;

                Console.WriteLine("Insert the Meter Firmware Version:");
                var FirmwareVersion = _userInput.ReadLine();

                if (FirmwareVersion.Length == 0)
                {
                    _logger.LogWarning("The Firmware Version can't be null, try again!");
                    return;
                }

                endpoint.MeterFirmwareVersion = FirmwareVersion;

                Console.WriteLine("Insert the Switch State:");
                var SwitchState = endpoint.SwitchState = EndpointModel.SetSwtichState(_userInput.ReadLine());

                if (SwitchState == -1)
                {
                    _logger.LogWarning("Switch State not found!");
                    return;
                }

                _memoryCache.TryGetValue(endpoint.EndpointSerialNumber, out EndpointModel endpointResult);

                if (endpointResult != null)
                {
                    _logger.LogError("Endpoint Serial Number already exists! Endpoint not created");
                    return;
                }

                _memoryCache.Set(endpoint.EndpointSerialNumber, endpoint);
                SerialNumberList.Add(endpoint.EndpointSerialNumber);

                _logger.LogInformation("Endpoint created!");
            }
            catch (Exception exception)
            {
                throw new Exception("There was a problem saving the endpoint!", exception);
            }
        }

        public void EditEndpointSwtichState(string SerialNumber)
        {
            try
            {
                _memoryCache.TryGetValue(SerialNumber, out EndpointModel endpoint);
                if (endpoint == null)
                {
                    _logger.LogWarning("Endpoint Serial Number not found!");
                    return;
                }
                Console.WriteLine("Insert the new switch state");
                endpoint.SwitchState = EndpointModel.SetSwtichState(_userInput.ReadLine());

                _logger.LogInformation("Switch state successfully changed!");
            }
            catch (Exception exception)
            {
                throw new Exception("There was a problem editing the endpoint!", exception);
            }

        }

        public void DeleteEndpoint(string SerialNumber)
        {
            try
            {
                _memoryCache.TryGetValue(SerialNumber, out EndpointModel endpoint);

                if (endpoint == null)
                {
                    _logger.LogWarning("Endpoint Serial Number not found!");
                    return;
                }
                Console.WriteLine($"Do you want to delete the Endpoint {endpoint.EndpointSerialNumber}");
                Console.WriteLine("(1) - Yes / (2) - No");
                var userInput = _userInput.ReadLine();

                if (userInput == "1")
                {
                    _memoryCache.Remove(SerialNumber);
                    SerialNumberList.Remove(SerialNumber);

                    _logger.LogInformation($"Endpoint {endpoint.EndpointSerialNumber} successfully deleted!");
                }
                else if (userInput == "2")
                {
                    _logger.LogWarning("Operation cancelled!");
                    return;
                }
                else
                {
                    _logger.LogWarning("Command not found!");
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An error happened during the exclusion.", exception);
            }
        }

        public void ListAllEndpoints()
        {
            if (SerialNumberList.Count == 0)
            {
                _logger.LogWarning("No endpoints found!");
            }

            SerialNumberList.ForEach(SerialNumber =>
            {
                _memoryCache.TryGetValue(SerialNumber, out EndpointModel endpoint);
                ShowEndpointInformation(endpoint);
                Console.WriteLine();
            });
        }

        public EndpointModel FindEndpointBySerialNumber(string SerialNumber)
        {
            _memoryCache.TryGetValue(SerialNumber, out EndpointModel endpoint);

            if (endpoint == null)
            {
                _logger.LogWarning("Endpoint Serial Number not found!");
                return null;
            }

            ShowEndpointInformation(endpoint);

            return endpoint;
        }

        public void ShowEndpointInformation(EndpointModel endpoint)
        {
            Console.WriteLine($"Endpoint Serial Number: {endpoint.EndpointSerialNumber}");
            Console.WriteLine($"Meter Model Id: {endpoint.MeterModelId}");
            Console.WriteLine($"Meter Number: {endpoint.MeterNumber}");
            Console.WriteLine($"Meter Firmware Version: {endpoint.MeterFirmwareVersion}");
            Console.WriteLine($"Switch State: {endpoint.SwitchState}");
        }
    }
}
