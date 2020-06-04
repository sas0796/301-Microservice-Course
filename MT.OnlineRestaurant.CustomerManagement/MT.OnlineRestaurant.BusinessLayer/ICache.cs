namespace MT.OnlineRestaurant.BusinessLayer
{
    public interface ICache
    {
        void SetInCache<T>(string key, T data);
        T GetFromCache<T>(string key);
        void InValidateCache(string key);
    }
}
