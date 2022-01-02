namespace Mobile.Core.Api
{
    public interface IApiService<IRestApi>
    {
        IRestApi GetClient();
    }
}
