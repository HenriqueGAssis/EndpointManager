using EndpointManager.Model;

namespace EndpointManager.Service
{
    public interface IEndpointService
    {
        void DeleteEndpoint(string SerialNumber);
        void EditEndpointSwtichState(string SerialNumber);
        EndpointModel FindEndpointBySerialNumber(string SerialNumber);
        void InsertNewEndpoint();
        void ListAllEndpoints();
        void ShowEndpointInformation(EndpointModel endpoint);
    }
}