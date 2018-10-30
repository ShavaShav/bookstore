using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BookStoreLib
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void InitializeTests(TestContext context)
        {
            // Reset database before tests run
            string connString = Properties.Settings.Default.DatabaseConnectionString;

            string script = File.ReadAllText("db_create.sql");
            
            // split script on GO command
            System.Collections.Generic.IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                     RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        using (var command = new SqlCommand(commandString, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
