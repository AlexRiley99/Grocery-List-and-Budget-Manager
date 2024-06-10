/******************************************
Name: Alex Riley
Date: 05/26/2024 - Last Edited on 06/09/2024
Assignment CIS317 Course Project Part 5
*
Main application class.
- Instantiates a list of "Item" objects
- I made all methods static because they aren't called using in instance of a class
- I also made all methods private because they won't be accessed from any other class

- Edits 05/30/2024
    - Addition of the ListDB and SQLiteDatabase classes
    - PopulateItemList()
    - Removed DisplayList() and replaced it with ListDB.GetAllItems()
    - Removed DeleteItem() and replaced it with ListDB.DeleteItem()
    - Removed EditItem() and replaced it with ListDB.UpdateItem()
- Edits 06/07/2024
    - Re-created DeleteItem(), this time in a way that passes itemNum to 
    ListDB.DeleteItem()
    - Re-created EditItem(), this time in a way that passes i to 
    ListDB.UpdateItem()
- Edits 06/09/2024
    - Addition of TextFile class
    - Incorporated text file data storage/retrieval
*/
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class GroceryListProgram{
    public static void Main(string[] args){
        const string dbName = "GroceryList.db";
        Console.WriteLine("\nAlex Riley - CIS317 Course Project Part 5\n");
        SQLiteConnection conn = SQLiteDatabase.Connect(dbName);

        if(File.Exists(dbName)){//Deletes existing database if one exists
            SQLiteDatabase.CloseConnection(conn);//Close connection
            File.Delete(dbName);
        }

        //Create new SQLite connection
        conn = SQLiteDatabase.Connect(dbName);

        if(conn != null){ //Creates new table 
            ListDB.CreateTable(conn);
        }


        Console.Write("Welcome! Would you like to create a grocery list? Y/N: ");

        bool validYN = false;
        string? YN = Console.ReadLine();
        do{ //do...while to continue prompting the user for a valid response if an invalid response is entered
            if(!string.IsNullOrEmpty(YN) && YN.ToUpper() == "Y"){ //if YN is not null or empty, and YN = "Y", execute the code block
                Console.WriteLine("\n\nAwesome! Let's start by budgeting for the grocery trip!\n");

                List<Item> GroceryList = new List<Item>(); //Instantiates a grocery list

                float groceryBudget = AskForBudget();//Gets the budget from the user

                if(conn != null){//Makes sure conn is not null because I was getting a warning about it
                    PopulateItemList(conn, GroceryList); //Gathers all the list values from the user
                    GroceryList = ListDB.GetAllItems(conn);//Update GroceryList with items from the 
                    //database and display GroceryList
                }
                
                float budget = GetBudget(groceryBudget); //Assigns the value of groceryBudget to budget
                float total = CalculateTotal(GroceryList); //Assigns the return value of CalcualteTotal() to total
                float difference = CalculateDifference(total, groceryBudget); //Assigns the return value of CalculateDifference() to difference

                Console.WriteLine("\nCalculations:");
                Console.WriteLine($"Your budget was: ${budget:F2}");
                Console.WriteLine($"Your total is: ${total:F2}");
                Console.WriteLine($"The difference between your budget and your total is: ${difference:F2}\n"); //Displays the budget, total, and difference

                string overUnder = OverUnderBudget(difference);
                Console.WriteLine($"{overUnder}"); //Tells the user if they're over, under, or on budget

                bool validEditYN = false;
                Console.Write("\nDo you want to make any edits to your list? Y/N: ");
                string? editYN = Console.ReadLine();
                do{ //do...while to coninue prompting the user for a valid response if an invalid one is entered
                    if(!string.IsNullOrEmpty(editYN) && editYN.ToUpper() == "Y"){

                        bool cancel = false;
                            do{ //do...while to stop the program if the user enters cancel, but continue looping otherwise
                                //this allows the user to edit or delete multiple items if they want to
                                bool validED = false;
                                validEditYN = true;
                                do{
                                    Console.WriteLine();
                                    Console.Write("Type 'edit','delete', or 'cancel': ");
                                    string ?editDelete = Console.ReadLine(); //Get "edit", "delete", or "cancel" from the user
                                        if(!string.IsNullOrEmpty(editDelete) && editDelete.ToLower() == "edit"){
                                            if (conn != null){//Makes sure conn is not null because I was getting a warning about it
                                                EditItem(conn, GroceryList); //Edit the list 
                                                Console.WriteLine("Here is your new list:\n");
                                                GroceryList = ListDB.GetAllItems(conn); //Get updated list

                                                budget = GetBudget(groceryBudget);
                                                total = CalculateTotal(GroceryList);
                                                difference = CalculateDifference(total, groceryBudget);

                                                Console.WriteLine("\nCalculations:"); //Display new monetary values
                                                Console.WriteLine($"Your budget was: ${budget:F2}");
                                                Console.WriteLine($"Your total is: ${total:F2}");
                                                Console.WriteLine($"The difference between your budget and your total is: ${difference:F2}\n");

                                                overUnder = OverUnderBudget(difference);
                                                Console.WriteLine($"{overUnder}"); //Display new budget status
                                                validED = true;
                                                cancel = false;
                                            }
                                        }
                                        else if(!string.IsNullOrEmpty(editDelete) && editDelete.ToLower() == "delete"){
                                            if(conn != null){//Makes sure conn is not null because I was getting a warning about it
                                                DeleteItem(conn, GroceryList); //Delete item
                                                Console.WriteLine("Here is your new list:\n");
                                                GroceryList = ListDB.GetAllItems(conn); //Get updated list

                                                budget = GetBudget(groceryBudget);
                                                total = CalculateTotal(GroceryList);
                                                difference = CalculateDifference(total, groceryBudget);

                                                Console.WriteLine("\nCalculations:"); //Display new monetary values
                                                Console.WriteLine($"Your budget was: ${budget:F2}");
                                                Console.WriteLine($"Your total is: ${total:F2}");
                                                Console.WriteLine($"The difference between your budget and your total is: ${difference:F2}\n");

                                                overUnder = OverUnderBudget(difference);
                                                Console.WriteLine($"{overUnder}"); //Display new budget status
                                                validED = true;
                                                cancel = false;
                                            }
                                        }
                                        else if(!string.IsNullOrEmpty (editDelete) && editDelete.ToLower() == "cancel"){ 
                                            Console.WriteLine("\nGoodbye!"); //Prints "Goodbye!" end stops the program if the user enters "Cancel"
                                            cancel = true;
                                            if(conn != null){
                                                TextFile.WriteToFile(conn); //Writes completed grocery list to text file
                                            }

                                            bool validViewYN = false;
                                            Console.Write("View text file? Y/N: "); //
                                            string? viewYN = Console.ReadLine();
                                                do{
                                                    
                                                    if(!string.IsNullOrEmpty(viewYN) && viewYN.ToUpper() == "Y"){
                                                        TextFile.ReadFromFile();
                                                        validViewYN = true;
                                                    }
                                                    else if(!string.IsNullOrEmpty(viewYN) && viewYN.ToUpper() == "N"){
                                                        Console.WriteLine("Goodbye!");
                                                        validViewYN = true;
                                                    }
                                                    else{
                                                        Console.WriteLine("Invalid response! Please only enter Y or N.");
                                                    }
                                                }while(!validViewYN);
                                                validEditYN = true;
                                            validED = true;
                                        }
                                        else{
                                            Console.WriteLine("Invalid response! Please only type 'edit', 'delete', or 'cancel'.");
                                        }
                                }while(!validED);
                            }while(!cancel);
 
                    }
                    else if(!string.IsNullOrEmpty(editYN) && editYN.ToUpper() == "N"){
                        if(conn != null){
                            TextFile.WriteToFile(conn); //Writes completed grocery list to text file
                        }

                        bool validViewYN = false;
                        Console.Write("View text file? Y/N: "); //
                        string? viewYN = Console.ReadLine();
                            do{
                                
                                if(!string.IsNullOrEmpty(viewYN) && viewYN.ToUpper() == "Y"){
                                    TextFile.ReadFromFile();
                                    validViewYN = true;
                                }
                                else if(!string.IsNullOrEmpty(viewYN) && viewYN.ToUpper() == "N"){
                                    Console.WriteLine("Goodbye!");
                                    validViewYN = true;
                                }
                                else{
                                    Console.WriteLine("Invalid response! Please only enter Y or N.");
                                }
                            }while(!validViewYN);
                            validEditYN = true;
                    }
                    else{
                        Console.WriteLine("Invalid response! Please only enter Y or N.");
                    }
                }while(!validEditYN);

                validYN = true;
            }
            else if(!string.IsNullOrEmpty(YN) && YN.ToUpper() == "N"){
                Console.WriteLine("\nGoodbye!");
                validYN = true;
            }
            else{
                Console.WriteLine("Invalid response! Please only enter Y or N.");
            }
        }while(!validYN);
    } 
    


    //Methods
    private static void PopulateItemList(SQLiteConnection conn, List<Item> GroceryList){ //Gathers information for the grocery list items
        bool listFinished = false;
        bool validInput = false;
        string ItemName = "";
        int ItemQuantity = 0;
        float ItemCost = 0;
    
        Console.WriteLine("\nLet's make your grocery list! Enter 'done' for 'Name of item' when finished.");
        do{ //do...while to allow the user to enter as many items as they want
            Console.Write("\nName of item: "); //Gets item name
            string? nameOfItem = Console.ReadLine();
                if(!string.IsNullOrEmpty(nameOfItem) && nameOfItem.ToLower() != "done"){ 
                    ItemName = nameOfItem;
                    do{ //do...while to continue prompting the user if they enter an invalid value
                        Console.Write("How many will you buy?: "); //Gets quantity
                        string? quantOfItem = Console.ReadLine();
                            if(int.TryParse(quantOfItem, out ItemQuantity)){
                                do{
                                    Console.Write("How much do you expect it to cost? (Per item): "); //Gets cost
                                    string? costOfItem = Console.ReadLine();
                                    if(float.TryParse(costOfItem, out ItemCost)){
                                        Item Item = new Item(0, ItemName, ItemQuantity, ItemCost);
                                        ListDB.AddItem(conn, Item);

                                        validInput = true;
                                    }
                                    else{
                                        Console.WriteLine("Oops! Please enter a valid numeric value.");
                                        validInput = false;
                                    }
                                }while(!validInput);
                                
                            }
                            else{
                                Console.WriteLine("Oops! Please enter a valid numeric value.");
                                validInput = false;
                            }
                            
                        }while (!validInput);
                    }
                    else{
                    Console.WriteLine("\nList finished!\n");
                    listFinished = true;
            } 
        }while(!listFinished);
    }
    
    private static float AskForBudget(){//Asks for the user's budget, and converts the input to a float value if the user entered a valid number
        bool validNumber = false;
        float groceryBudget;
        do{
            Console.Write("What is your budget for this grocery trip?: ");
            string? input = Console.ReadLine();
                if(float.TryParse(input, out groceryBudget)){
                    Budget budget = new Budget(groceryBudget);
                    Console.WriteLine($"\nBudget set successfully! Your budget is: ${groceryBudget}\n");
                    validNumber = true;
                }
                else{
                    Console.WriteLine("Oops! Please enter a valid numeric value.\n");
                    validNumber = false;
                }
        } while(!validNumber);

        return groceryBudget;
    }

    private static float GetBudget(float groceryBudget){
        return groceryBudget;
    }

    private static float CalculateTotal(List<Item> GroceryList){ //Calculates the total cost of all items in the list
        float total = 0;

            foreach (Item item in GroceryList){
                total += item.ItemCost * item.ItemQuantity; //Example: an item with a quantity of 2 and a cost of 1.50 would have a total of $3
            } //This way, the user can buy multiple of the same item, but only have to know the price of 1

        return total;
    }
    
    private static float CalculateDifference(float total, float groceryBudget){
        float difference = groceryBudget - total; //Subtracts the user's total from the original budget amount to see how much they're spending

        return difference;
    } 

    private static string OverUnderBudget(float difference){ //Tells the user if they're over, under, or on budget
        string overUnder = "";

        if(difference < 0){
            overUnder = "You are over budget!";
        }
        else if(difference > 0){
            overUnder = "You are under budget!";
        }
        else{
            overUnder = "You are right on budget!";
        }

        return overUnder;
    }

    private static void EditItem(SQLiteConnection conn, List<Item> GroceryList){//Edits items
        bool validNumber = false;
        int itemNum = 0;
        do{
            Console.Write("Please enter the number of the item you would like to edit: ");
            string? input = Console.ReadLine();
            if(int.TryParse(input, out itemNum)){
                Item item = ListDB.GetItem(conn, itemNum);
                if(item.ItemNum > 0 && item.ItemNum <= GroceryList.Count){
                    item.Edit();
                    ListDB.UpdateItem(conn, item);
                    
                    validNumber = true;
                } 
            }
            else{
                Console.WriteLine("Oops! Please enter a valid numeric value.\n");
            }
        } while(!validNumber);
    }

    private static void DeleteItem(SQLiteConnection conn, List<Item> GroceryList){//Deletes items
        bool validNumber = false;
        int itemNum = 0;
        do{
            Console.Write("Please enter the number of the item you would like to delete: ");
            string? input = Console.ReadLine();
            if(int.TryParse(input, out itemNum)){
                ListDB.DeleteItem(conn, itemNum);

                Console.WriteLine("Item deleted!\n");
                validNumber = true;
            }
            else{
                Console.WriteLine("Oops! Please enter a valid numeric value.\n");
            }
        } while (!validNumber);
    }
}