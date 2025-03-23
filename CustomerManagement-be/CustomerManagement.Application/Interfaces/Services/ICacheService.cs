namespace CudstomerManagement.Application.Interfaces;

public interface ICacheService
{
    void Set<T>(string key, T value, TimeSpan duration);
    void Remove(string key);
    bool TryGet<T>(string key, out T? value);
}
