using System;
using System.Collections.Generic;

namespace MTech.FileArchiver
{
    public class Security
    {

        public static string AddUser(string username, string password)
        {
            // Create Hash Code
            string combine = username + password;
            //byte[] bytes = System.Text.Encoding.Unicode.GetBytes(combine);
            //System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            //bytes = md5.ComputeHash(bytes);
            //combine = System.Text.Encoding.ASCII.GetString(bytes);
            combine = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(combine, "md5");

            // Register
            if (System.Web.HttpContext.Current.Application["FileArchiverUsers"] != null)
            {
                ((List<string>)System.Web.HttpContext.Current.Application["FileArchiverUsers"])
                    .Add(combine);
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(combine);
                System.Web.HttpContext.Current.Application["FileArchiverUsers"] = list;
            }

            return combine;
        }

        public static void RemoveUser(string hashcode)
        {
            List<string> users = ((List<string>)System.Web.HttpContext.Current.Application["FileArchiverUsers"]);
            if (users != null) users.Remove(hashcode);
        }

        public static bool ContainsUser(string hashcode)
        {
            List<string> users = ((List<string>)System.Web.HttpContext.Current.Application["FileArchiverUsers"]);
            if (users != null)
                return users.Contains(hashcode);
            else
                return false;
        }

        public static bool IsEnabled()
        {
            string enabled = System.Configuration.ConfigurationManager.AppSettings["FileArchiver.Security.Enabled"];
            if (!string.IsNullOrEmpty(enabled) && enabled == "1")
                return true;
            return false;
        }

        public static bool CheckSecurity(string hashcode)
        {
            bool allow = true;
            if (IsEnabled())
            {
                allow = false;
                if (hashcode != "" && ContainsUser(hashcode))
                    allow = true;
            }
            return allow;
        }

    }
}
