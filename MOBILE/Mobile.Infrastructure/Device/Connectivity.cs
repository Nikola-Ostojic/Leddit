using Plugin.Connectivity;

namespace Mobile.Infrastructure.Device
{
    public class Connectivity : IConnectivity
    {
        public bool IsConnected => CrossConnectivity.Current.IsConnected;
    }
}
