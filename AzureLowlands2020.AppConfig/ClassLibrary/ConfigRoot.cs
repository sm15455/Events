namespace ClassLibrary
{
  public class ConfigRoot
  {
    public string Background { get; set; }
    public SMTP SMTP { get; set; }
    public SMTP SMTPJson { get; set; }
  }

  public class SMTP
  {
    public string Server { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }

}