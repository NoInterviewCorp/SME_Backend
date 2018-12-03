public class RabbitMQSettings
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
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsDockerized { get; set; }
}