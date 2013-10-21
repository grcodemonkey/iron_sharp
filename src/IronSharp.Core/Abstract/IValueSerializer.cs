namespace IronSharp.Core
{
    public interface IValueSerializer
    {
        string Generate(object value);

        T Parse<T>(string value);
    }
}