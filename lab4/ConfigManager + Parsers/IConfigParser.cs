namespace ConfigProvider
{
    public interface IConfigParser
    {
        public T Parse<T>(string configFileName) where T : new();
    }
}