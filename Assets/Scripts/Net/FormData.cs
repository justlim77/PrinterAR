public class LoginData
{
    public string username = "";
    public string password = "";

    public void Clear()
    {
        username = "";
        password = "";
    }
}

public class RegistrationData
{
    public string username = "";
    public string password = "";
    public string email = "";
    public string company = "";

    public void Clear()
    {
        username = "";
        password = "";
        email = "";
        company = "";
    }
}

public class Response
{
    public bool error = false;
    public string message = "";
}