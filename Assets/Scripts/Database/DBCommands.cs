﻿using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public static class DBCommands
    {
        public static string get_ifexists_username = "";

        public const string get_all_register_params = "SELECT * FROM dbo.tblRegister";
        public const string get_all_salesinfo_params = "SELECT * FROM dbo.tblSalesInfo";
        public const string get_all_postalcode_params = "SELECT * FROM dbo.tblPostalcode";

        public const string get_register_params_withuser = "SELECT * FROM dbo.tblRegister WHERE CUserID=@CUserID";
        public const string get_postalcode_params_withcode = "SELECT * FROM dbo.tblPostalcode WHERE code=@code";

        public static void InitCommands()
        {
            // TODO
        }     
    }
}