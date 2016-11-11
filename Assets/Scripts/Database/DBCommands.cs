using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public static class DBCommands
    {
        public static string get_ifexists_username = "";

        public const string get_all_register_params = "SELECT * FROM dbo.tblRegister";
        public const string get_all_salesinfo_params = "SELECT * FROM dbo.tblSalesInfo";
        public const string get_all_postalcode_params = "SELECT * FROM dbo.tblPostalcode";

        public const string get_register_params_withCName = "SELECT * FROM dbo.tblRegister WHERE CName=CName";// AND CONSTAINT_TYPE = 'PRIMARY KEY'";
        //public const string get_register_params_withPrimaryKey = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE a WHERE EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS b WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME) AND TABLE_NAME = @dbo.tblRegister";
        public const string get_register_params_withPrimaryKey = "SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC WHERE TC.TABLE_NAME = 'tblRegister' AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'";

        public const string get_register_params_withCID = "SELECT * FROM dbo.tblRegister WHERE CID=@CID";
        public const string get_postalcode_params_withcode = "SELECT * FROM dbo.tblPostalcode WHERE code=@code";

        public const string insert_register_params = "INSERT INTO dbo.tblRegister (CID, CName, Company, CPwd, Email) VALUES (@CID, @CName, @Company, @CPwd, @Email)";

        public static void InitCommands()
        {
            // TODO
        }     
    }
}
