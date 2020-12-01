namespace lab3
{
    public interface IConfigParser
    {
        public T Parse<T>(string configFileName) where T : new();
    }
}