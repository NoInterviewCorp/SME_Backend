public class MongoSettings
{
    private string connectionString = string.Empty;
    public string ConnectionString
    {
        get
        {
            if (IsDockerized && IsInDevelopment)
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
    public bool IsInDevelopment { get; set; }
}