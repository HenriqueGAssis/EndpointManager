using EndpointManager.Service;

namespace EndpointManager.UserInterface
{
    class Menu : IMenu
    {
        private readonly IEndpointService _endpointService;

        public Menu(IEndpointService endpointService)
        {
            _endpointService = endpointService;
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome! Choose an option to continue:");
            Console.WriteLine("(1) - Insert a new endpoint");
            Console.WriteLine("(2) - Edit an existing endpoint");
            Console.WriteLine("(3) - Delete an existing endpoint");
            Console.WriteLine("(4) - List all endpoints");
            Console.WriteLine("(5) - Find a endpoint by Serial Number");
            Console.WriteLine("(6) - Exit");
        }

        public void ShowUserInteraction()
        {
            bool exit = false;

            while (!exit)
            {
                ShowMenu();
                var userOption = Console.ReadLine();

                switch (userOption)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("ADD A NEW ENDPOINT");

                        _endpointService.InsertNewEndpoint();
                        Console.ReadLine();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("EDIT AN EXISTING ENDPOINT");
                        Console.WriteLine("Insert the Endpoint Serial Number:");

                        _endpointService.EditEndpointSwtichState(Console.ReadLine());
                        Console.ReadLine();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("DELETE AN EXISTING ENDPOINT");
                        Console.WriteLine("Insert the Endpoint Serial Number:");

                        _endpointService.DeleteEndpoint(Console.ReadLine());
                        Console.ReadLine();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("LIST ALL ENDPOINTS");

                        _endpointService.ListAllEndpoints();
                        Console.ReadLine();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("FIND A ENDPOINT BY SERIAL NUMBER");
                        Console.WriteLine("Write the Endpoint Serial Number:");

                        _endpointService.FindEndpointBySerialNumber(Console.ReadLine());
                        Console.ReadLine();
                        break;
                    case "6":
                        Console.WriteLine("Do you want to exit?");
                        Console.WriteLine("(1) - Yes / (2) - No");

                        if (Console.ReadLine() == "1")
                        {
                            exit = true;
                        }
                        break;
                    default:
                        Console.WriteLine("Command not found, try again!");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
