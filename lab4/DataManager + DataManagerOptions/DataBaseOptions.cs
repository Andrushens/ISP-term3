namespace DMOptions
{
    public class DataBaseOptions
    {
        public string ConnectionString { get; set; }

        public DataBaseOptions()
        {
            ConnectionString = @"Server = KOROLIVA; Database = AdventureWorks2019; Trusted_Connection = True;";
        }
    }
}
