using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
//using Mono.Data.Sqlite;
using I18N;
using I18N.CJK;
using I18N.Common;
using I18N.MidEast;
using I18N.Other;
using I18N.Rare;
using I18N.West;

namespace CopierAR
{
    public static class DBManager
    {
        public static string CONN_STRING
        {
            get
            {
                return @"Data Source=SQL5028.SmarterASP.NET;Initial Catalog = DB_A12D5C_Copier; User Id = DB_A12D5C_Copier_admin; Password=Copier123;";
            }
        }

        static SqlConnection DBCONN = null;

        public static void Initialize()
        {
            if (DBCONN == null)
            {
                DBCONN = (SqlConnection)new SqlConnection(CONN_STRING);
                try
                {
                    Debug.Log("Opening database...");
                    DBCONN.Open();
                    DBCommands.InitCommands();
                    Debug.Log("Database connection open");
                    //DebugLog.Log("Database connection open");
                }
                catch (SqlException e)
                {
                    Debug.Log(e.Message);
                    //DebugLog.Log(e.Message);
                }
                catch (UnityException e)
                {
                    Debug.Log(e.Message);
                    //DebugLog.Log(e.Message);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    //DebugLog.Log(e.Message);
                }
            }
        }

        public static void Uninitialize()
        {
            Debug.Log("Closing database connection");
            if (DBCONN != null)
                DBCONN.Close();
            DBCONN = null;
        }

        public static string GenerateSelectExists(string tableName, Type type)
        {
            FieldInfo[] infos = type.GetFields();
            string[] values = Array.ConvertAll<FieldInfo, string>(infos, x => { return x.Name; });
            List<string> valueNames = new List<string>(values);

            string columns = "";
            foreach (string v in valueNames)
            {
                columns += string.Format(" {0}=@{0} AND", v);
            }
            columns = columns.Remove(columns.Length - 3, 3);

            string command = string.Format("SELECT * FROM {0} WHERE ({1})", tableName, columns);
            return command;
        }

        public static string GenerateSelectExistsCommand(string tableName, string column)
        {
            return string.Format("SELECT COUNT(*) FROM {0} WHERE {1}=@{1}", tableName, column);
        }

        #region Tools
        delegate bool ProcessDBEvent(ref SqlTransaction transaction);
        static bool ProcessDB(ProcessDBEvent dbEvent)
        {
            Initialize();
            bool result = false;
            try
            {
                SqlTransaction transaction = DBCONN.BeginTransaction(IsolationLevel.Serializable);
                if (dbEvent(ref transaction))
                {
                    Debug.Log("Committing db transaction...");
                    transaction.Commit();
                    result = true;
                }
                else
                {
                    Debug.Log("Db transaction failed: Rolling back...");
                    transaction.Rollback();
                }
            }
            catch (SqlException e)
            {
                Debug.Log(e.Message);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            Uninitialize();
            return result;
        }
        #endregion

        #region Registration
        static void ReadRegistrationData(SqlDataReader reader, ref RegistrationData data)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.IsDBNull(i))
                    continue;

                string column = reader.GetName(i);
                switch (column)
                {
                    case "CID":
                        data.CID = reader.GetInt32(i);
                        break;
                    case "CName":
                        data.CName = reader.GetString(i);
                        break;
                    case "Company":
                        data.Company = reader.GetString(i);
                        break;
                    case "CPwd":
                        data.CPwd = reader.GetString(i);
                        break;
                    case "Email":
                        data.Email = reader.GetString(i);
                        break;
                    default:
                        Debug.Log("Invalid column name.");
                        break;
                }
            }
        }

        public static RegistrationData GetRegistrationData(string name)
        {
            RegistrationData data = new RegistrationData();
            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = DBCommands.get_register_params_withCName;
                    command.Parameters.Add(new SqlParameter("CName", name));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    data = new RegistrationData();
                        //    ReadRegistrationData(reader, ref data);
                        //    Debug.Log(string.Format("ReadData > {0} {1}", data.CName, data.CPwd));
                        //    break;
                        //}

                        int ordName = reader.GetOrdinal("CName");
                        int ordPwd = reader.GetOrdinal("CPwd");
                        //Debug.Log(string.Format("ordName: {0}, ordPwd: {1}", ordName, ordPwd));

                        if (!reader.Read())
                            throw new InvalidOperationException("No records were returend");

                        //ReadRegistrationData(reader, ref data);
                        data.CName = reader.GetString(ordName);
                        data.CPwd = reader.GetString(ordPwd);

                        Debug.Log(string.Format("ReadData > {0} {1}", data.CName, data.CPwd));

                        if (reader.Read())
                            throw new InvalidOperationException("Multiple records were returned");
                    }
                }
                return false;
            };
            ProcessDB(dbEvent);
            return data;
        }

        public static RegistrationData GetRegistrationData(int id)
        {
            RegistrationData data = new RegistrationData();
            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = DBCommands.get_register_params_withCID;
                    command.Parameters.Add(new SqlParameter("CID", id));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //while (reader.Read())
                        //{
                        //    data = new RegistrationData();
                        //    ReadRegistrationData(reader, ref data);
                        //    break;
                        //}

                        // New implementation
                        if (!reader.Read())
                            throw new InvalidOperationException("No records were returend");

                        
                    }
                }
                return false;
            };
            ProcessDB(dbEvent);
            return data;
        }

        public static bool CreateUser(RegistrationData data)
        {
            int n = 0;
            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = DBCommands.insert_register_params;
                    command.Parameters.AddWithValue("CName", data.CName);
                    command.Parameters.AddWithValue("Company", data.Company);
                    command.Parameters.AddWithValue("CPwd", data.CPwd);
                    command.Parameters.AddWithValue("Email", data.Email);

                    try
                    {
                        n = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (SqlException ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }

                    Debug.Log(n == 0 ? "Registration failed: Unable to insert row" : "Registration success: " + n);

                    // Commit transaction
                    transaction.Commit();
                    transaction = DBCONN.BeginTransaction(IsolationLevel.Serializable);
                }
                return (n != 0);
            };

            return ProcessDB(dbEvent);
        }

        #endregion

        #region Sales Info
        public static bool InsertSalesInfo(SalesInfoData infoData)
        {
            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = DBCommands.insert_salesinfo_params;
                    command.Parameters.AddWithValue("SName", infoData.SName);                       // string
                    command.Parameters.AddWithValue("PostalCod", infoData.PostalCod);               // decimal
                    command.Parameters.AddWithValue("LoginTime", infoData.LoginTime);               // DateTime
                    command.Parameters.AddWithValue("PhotoCopierModel", infoData.PhotoCopierModel); // csv string
                    command.Parameters.AddWithValue("DemoDuration", infoData.DemoDuration);         // string
                    command.Parameters.AddWithValue("Frequency", infoData.Frequency);               // csv string

                    bool result = false;
                    try
                    {
                        result = (command.ExecuteNonQuery() != 0);
                    }
                    catch (SqlException ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }

                    return result;
                }
            };

            return ProcessDB(dbEvent);
        }
        #endregion

        #region Postal Code
        public static bool CheckPostalExists(string code)
        {
            bool exists = false;

            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Parameters.Clear();
                    command.Transaction = transaction;
                    command.CommandText = GenerateSelectExistsCommand("dbo.tblPostalcode", "code");
                    command.Parameters.Add(new SqlParameter("code", code));

                    exists = (int)command.ExecuteScalar() > 0;

                    if (exists)
                    {
                        Debug.Log("Postal code exists.");
                        //DebugLog.Log("Postal code exists.");
                    }
                    else
                    {
                        Debug.Log("Postal code does not exist.");
                        //DebugLog.Log("Postal code does not exist.");
                    }
                }

                return exists;
            };

            ProcessDB(dbEvent);

            return exists;
        }
        public static bool UpsertPostalCodeData(string code)
        {
            bool exists = false;

            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Parameters.Clear();
                    command.Transaction = transaction;
                    //command.CommandText = GenerateSelectExistsCommand("dbo.tblPostalcode", "code");
                    command.CommandText = DBCommands.upsert_postalcode_params;
                    command.Parameters.Add(new SqlParameter("code", code));

                    //exists = (int)command.ExecuteScalar() > 0;
                    command.ExecuteNonQuery();

                    exists = true;

                    if (exists)
                    {
                        Debug.Log("Postal code exists.");
                        //DebugLog.Log("Postal code exists.");
                    }
                    else
                    {
                        Debug.Log("Postal code does not exist.");
                        //DebugLog.Log("Postal code does not exist.");
                    }
                }

                return exists;
            };

            ProcessDB(dbEvent);

            return exists;
        }

        static void ReadPostalCodeData(SqlDataReader reader, ref LocationData data)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.IsDBNull(i))
                    continue;

                string column = reader.GetName(i);
                switch (column)
                {
                    case "code":
                        data.code = reader.GetString(i);
                        break;
                    case "Postal_Name":
                        data.Postal_Name = reader.GetString(i);
                        break;
                    case "Postal_Code":
                        data.Postal_Code = reader.GetString(i);
                        break;
                    default:
                        Debug.Log("Invalid column name.");
                        break;
                }
            }
        }

        public static LocationData[] GetAllPostalCodeData()
        {
            List<LocationData> data = new List<LocationData>();
            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = DBCommands.get_all_postalcode_params;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LocationData ld = new LocationData();
                            ReadPostalCodeData(reader, ref ld);
                            data.Add(ld);
                        }
                    }
                }

                return true;
            };

            ProcessDB(dbEvent);
            return data.ToArray();
        }
        #endregion

        #region Login
        public static bool CheckUserExists(string username)
        {
            bool exists = false;

            ProcessDBEvent dbEvent = delegate (ref SqlTransaction transaction)
            {
                using (SqlCommand command = DBCONN.CreateCommand())
                {
                    //command.Parameters.Clear();
                    command.Transaction = transaction;
                    //command.CommandText = GenerateSelectExistsCommand("dbo.tblRegister", "CName");
                    command.CommandText = DBCommands.get_register_params_withCName;
                    command.Parameters.Add(new SqlParameter("CName", username));

                    exists = (int)command.ExecuteScalar() > 0;

                    if (exists)
                    {
                        Debug.Log("User already exists.");
                        //DebugLog.Log("User already exists.");
                    }
                    else
                    {
                        Debug.Log("User does not exist.");
                        //DebugLog.Log("User does not exist.");
                    }
                }

                return exists;
            };

            ProcessDB(dbEvent);

            return exists;
        }
        #endregion
    }
}