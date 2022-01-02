using Plugin.Connectivity;

namespace Mobile.Core.Device
{
    public class Connectivity : IConnectivity
    {
        public bool IsConnected => CrossConnectivity.Current.IsConnected;
    }
}
