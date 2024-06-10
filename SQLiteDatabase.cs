/******************************************
Name: Alex Riley
Date: 05/26/2024
Assignment CIS317 Course Project Part 3
*
SQLiteDatabase class.
- Connects to an existing database or creates one if one doesn't exist
*/
using System;
using System.Data.SQLite;

public class SQLiteDatabase{
    public static SQLiteConnection Connect(string database){
        string cs = @"Data Source=" + database;
        SQLiteConnection conn = new SQLiteConnection(cs);

        try{
            conn.Open();
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }

        return conn;
    }

    public static void CloseConnection(SQLiteConnection conn){
        if(conn != null){
            conn.Close();
            conn.Dispose();
        }
    }
}