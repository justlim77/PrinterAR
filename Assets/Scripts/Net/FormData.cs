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
    public int CID = 0;
    public string CName = "";
    public string Company = "";
    public string CUserID = "";
    public string CPwd = "";
    public string CopierModel = "";
    public string Frequency = "0";
    public decimal PostalCode = 0;
    public string Email = "";

    public void Clear()
    {
        CID = 0;
        CName = "";
        Company = "";
        CUserID = "";
        CPwd = "";
        CopierModel = "";
        Frequency = "0";
        PostalCode = 0;
        Email = "";
    }
}

public class Response
{
    public bool error = false;
    public string message = "";
}