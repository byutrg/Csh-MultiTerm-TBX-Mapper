﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace MultiTermTBXMapper
{
    class TBXDatabase
    {
        protected static SQLiteConnection con;

        public static void Initialize()
        {
            con = new SQLiteConnection("Data Source=tbx_datcats.sqlite;Version=3;");
            con.Open();

            if(IsDBEmpty())
            {
                BuildDB();
            }
        }

        private static bool IsDBEmpty()
        {
            bool ret = false;

            string count_tables = "select count (*) from categories";

            SQLiteCommand command = new SQLiteCommand(count_tables, con);

            try
            {
                SQLiteDataReader results = command.ExecuteReader();
            }
            catch (SQLiteException e) { ret = true; }


            return ret;
        }

        private static void BuildDB()
        {
            string script = File.ReadAllText(@"Resources/tbx_data_categories_with_definitions.sql");
            SQLiteCommand command = new SQLiteCommand(script, con);
            command.ExecuteNonQuery();
        }

        public static void close()
        {
            con.Close();
        }

        public static void open()
        {
            con.Open();
        }

        public static List<string[]> getAll()
        {
            string query = "select name,element,description from categories";

            SQLiteCommand command = new SQLiteCommand(query, con);

            SQLiteDataReader reader = command.ExecuteReader();

            List<string[]> datcats = new List<string[]>();

            while(reader.Read())
            {
                string[] dc = new string[3] { (string) reader["name"], (string) reader["element"], (string) reader["description"] };
                datcats.Add(dc);
            }

            return datcats;
        }

        public static List<string[]> getNames()
        {
            string query = "select name from categories";

            SQLiteCommand command = new SQLiteCommand(query, con);

            SQLiteDataReader reader = command.ExecuteReader();

            List<string[]> datcats = new List<string[]>();

            while (reader.Read())
            {
                string[] dc = new string[1] { (string)reader["name"] };
                datcats.Add(dc);
            }

            return datcats;
        }
    }
}