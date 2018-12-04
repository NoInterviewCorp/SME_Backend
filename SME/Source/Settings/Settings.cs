public class MongoSettings
{
    private string connectionString = string.Empty;
    public string ConnectionString
    {
        get
        {
            if (IsDockerized)
            {
                return Container;
            }
            else
            {
                return connectionString;
            }
        }
        set
        {
            connectionString = value;
        }
    }
    public string Container { get; set; }
    public string Database { get; set; }
    public bool IsDockerized { get; set; }
}