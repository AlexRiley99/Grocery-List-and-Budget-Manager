/******************************************
Name: Alex Riley
Date: 05/26/2024
Assignment CIS317 Course Project Part 5
*
TextFile class.
Writes to a text file and prints the text from the file if the user wants to see it
*/

using System.Data.SQLite;
using System.IO;

public class TextFile{
    private const string FILE_NAME = "GroceryList.txt";
    SQLiteConnection conn = SQLiteDatabase.Connect("GroceryList.db"); //Connect to database

    //Write to file
    public static void WriteToFile(SQLiteConnection conn){
        List<Item> items = ListDB.GetAllItems(conn);
        string content = Environment.NewLine + "Grocery List:" + Environment.NewLine + "-------------" + Environment.NewLine;
        foreach (var item in items){           
            content += item.ToString() + Environment.NewLine;
        }//Adds each list item line by line in string format

        File.WriteAllText(FILE_NAME, content);
        Console.WriteLine("Completed grocery list saved successfully in text file!\n");
        }

    //Read from file
    public static void ReadFromFile(){
        //Read all lines from GroceryList.txt into a string array
        string[] readText = File.ReadAllLines(FILE_NAME);

        //Print each line
        foreach (string s in readText){
            Console.WriteLine(s);
        }

        Console.WriteLine("*End of file*");
    }
}