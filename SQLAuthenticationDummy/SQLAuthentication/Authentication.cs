using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace SQLAuthentication
{

    // Host : https://my.gearhost.com/Databases/Details/netauth

    class Authentication
    {
        static void Main(string[] args)
        {

            bool test = Authorize("efren", "urena");
            bool test2 = Authorize("efren2", "urena2");

            CreateAccount("efren2", "urena2");
            test2 = Authorize("efren2", "urena2");
        }

        //Authorize returns true if authentication was successful
        static bool Authorize(string username, string pass)
        {
            User user;
            using (var db = new AuthContext())
            {
                user = (from all in db.Users
                        where all.username == username
                        where all.pass == pass //here getHash()
                        select all).FirstOrDefault();
            }
            return (user == null) ? false : true;
        }

        //CreateAccount returns an exception if username already exists
        static void CreateAccount(string username, string pass)
        {
            User userMatch;
            User user = new User { username = username, pass = pass }; //here getHash();
            using (var db = new AuthContext())
            {
                userMatch = (from all in db.Users
                         where all.username == username
                         select all).FirstOrDefault();
                if (userMatch != null && userMatch.username == username)
                {
                    throw new Exception("Username already exists");
                }
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
