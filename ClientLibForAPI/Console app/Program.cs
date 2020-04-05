using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using ApiFormat;
using Newtonsoft.Json;

namespace ClientLibForAPI
{
    class program
    {

        private static void Main()
        {

            /* TEST af funktionalitet for shoppinglist*/

            Console.WriteLine("Hello world");
            var Shoppinglistforid1 = SerializerShoppingList.GetShoppingListByGroupId(1).Result;

            foreach (var i in Shoppinglistforid1)
            {
                Console.WriteLine($"{i.shoppingListGroupID}, {i.shoppingListID}, {i.shoppingListName}");
            }
            Console.WriteLine("SHOPPINGLIST BY ID:\n");
            var shoppinglistByID = SerializerShoppingList.GetShoppingListByShoppinglistId(1).Result;

            Console.WriteLine($"{shoppinglistByID.shoppingListID}, {shoppinglistByID.shoppingListGroupName}, {shoppinglistByID.shoppingListName}");

            Console.WriteLine("ITEMS:\n");
            foreach (var i in shoppinglistByID.Items)
            {
                Console.WriteLine($"{i.Name}, {i.Amount}, {i.Type}");

            }

            shoppinglistByID.shoppingListName = "This Works very well :3";

            var d = SerializerShoppingList.PostShoppingList(shoppinglistByID).Result;


            var testerforItem = SerializerShoppingList.GetShoppingListByShoppinglistId(3).Result;
            testerforItem.Items.Add(new ItemDTO()
            {
                ItemID = 1,
                Name = "Banan",
                Type = "Frugt",
                Amount = 3

               
            });

            var NewShoppinglist = SerializerShoppingList.PostShoppingList(testerforItem).Result;
            
           
            Console.WriteLine($"{NewShoppinglist.shoppingListID}, {NewShoppinglist.shoppingListGroupName}, {NewShoppinglist.shoppingListName}");
            



            //Console.WriteLine($"{Shoppinglistforid9.shoppingListID}, {Shoppinglistforid9.shoppingListGroupName}, {Shoppinglistforid9.shoppingListName}");
        }

    }
}