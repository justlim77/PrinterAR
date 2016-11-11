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

        //public const string insert_register_params = "INSERT INTO dbo.tblRegister (CID, CName, Company, CPwd, Email) VALUES ((SELECT ISNULL(MAX(CID)+1, 0) FROM dbo.tblRegister with (SERIALIZABLE, UPDLOCK)), @CName, @Company, @CPwd, @Email)";
        //public const string insert_register_params = "INSERT INTO dbo.tblRegister (CName, Company, CPwd, Email) VALUES (@CName, @Company, @CPwd, @Email); SELECT IDENT_CURRENT('dbo.tblRegister');";
        public const string insert_register_params = "MERGE dbo.tblRegister WITH (HOLDLOCK) AS r "
            + "USING(SELECT @CName AS CName, @Email as Email) AS new_r ON r.CName = new_r.CName AND r.Email = new_r.Email "
            + "WHEN NOT MATCHED THEN INSERT (CName, Company, CPwd, Email) "
            + "VALUES(new_r.CName, @Company, @CPwd, new_r.Email); "
            + "SELECT IDENT_CURRENT('dbo.tblRegister');";

        public static void InitCommands()
        {
            // TODO
        }     
    }
}

/*  MSSQL Commands
    DECLARE @CName varchar(50) = 'Jespa'
    DECLARE @Company nvarchar(100) = 'iOosh'
    DECLARE @CPwd nvarchar(100) = 'janice'
    DECLARE @Email varchar(50) = 'jespa@ioosh.com.sg'

    MERGE dbo.tblRegister WITH (HOLDLOCK) AS f
    USING (SELECT @CName AS CName, @Email as Email) AS new_register
    ON f.CName = new_register.CName AND f.Email = new_register.Email
    WHEN MATCHED THEN 
    UPDATE SET f.Email = 'update@email.com'
    WHEN NOT MATCHED THEN INSERT (CName, Company, CPwd, Email) VALUES (new_register.CName, @Company, @CPwd, new_register.Email);
    Selection
    SELECT * from dbo.tblRegister

    Update
    UPDATE dbo.tblRegister
    SET Email='jespa@ioosh.com.sg', CPwd='janice'
    WHERE CName='Jespa';

    Delete from
    DELETE FROM dbo.tblRegister
    WHERE CName='Jespa' AND Email='jespa@ioosh.com.sg';

    Delete ALL CAUTION
    DELETE FROM dbo.tblRegister;

    RESEED auto-increment
    DBCC CHECKIDENT (tblRegister, RESEED, 0)
 * */
