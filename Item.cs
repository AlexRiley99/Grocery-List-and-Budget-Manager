/******************************************
Name: Alex Riley
Date: 05/26/2024 - Last Edited on 05/30/2024
Assignment CIS317 Course Project Part 3
*
Item class.
- Inherits from List and contains the constructor for Item objects, which will then
be compiled into a grocery list in the main program

- Edits 05/30/2024: 
    - ItemNum property added for the database primary key
    - Removed "public List<Item> ItemList {get; set;}", as it was no longer 
    necessary and was interfering with the code
- Edits 06/09/2024:
    - Added ToString method because the list wasn't printing when ListDB.GetAllItems
    was called
*/
using System.Runtime.CompilerServices;

public class Item : List{
    public int ItemNum { get; set; }
    public string ItemName { get; set; }
    public int ItemQuantity { get; set; }
    public float ItemCost { get; set; }

    //Constructor
    public Item(int itemNum, string itemName, int itemQuantity, float itemCost){
        ItemNum = itemNum;
        ItemName = itemName;
        ItemQuantity = itemQuantity;
        ItemCost = itemCost;
    }

//Methods
    public override string ToString(){
        return $"{ItemNum}. {ItemName}\n   {ItemQuantity}\n   ${ItemCost:F2}\n";
    }
    //Inherited from List
    public override void Edit(){ //Gathers the new information for the edited item from the user
        
        Console.WriteLine($"\nCurrent Item: \nItem: {ItemName}\nQuantity: {ItemQuantity}\nCost: {ItemCost}\n");
        Console.WriteLine("Please enter the new information for this item.");
        Console.Write("New Item: ");
            string? newName = Console.ReadLine();
        Console.Write("New Quantity: ");
            string? NewQuantity = Console.ReadLine();
            int.TryParse(NewQuantity, out int newQuantity);
        Console.Write("New Cost: ");
            string? NewCost = Console.ReadLine();
            float.TryParse(NewCost, out float newCost);

        ItemName = !string.IsNullOrEmpty(newName) ? newName : ItemName;
        ItemQuantity = newQuantity;
        ItemCost = newCost;

        Console.WriteLine("\nItem updated successfully!\n");
    }
        
}
