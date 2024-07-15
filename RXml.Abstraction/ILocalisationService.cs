namespace RXml.Abstraction;

public interface ILocalisationService<T>
{
    string this[string key] { get; }
    string GetString(string key);
}