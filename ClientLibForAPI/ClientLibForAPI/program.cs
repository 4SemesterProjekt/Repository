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
            Shoppinglist tester = new Shoppinglist();
            var Shoppinglistforid1 = tester.GetShoppingListByGroupId(1).Result;
            
            foreach(var i in Shoppinglistforid1) 
            {
                Console.WriteLine($"{i.shoppingListGroupID}, {i.shoppingListID}, {i.shoppingListName}");
            }
            Console.WriteLine("SHOPPINGLIST BY ID:\n");
            var shoppinglistByID = tester.GetShoppingListByShoppinglistId(1).Result;

            Console.WriteLine($"{shoppinglistByID.shoppingListID}, {shoppinglistByID.shoppingListGroupName}, {shoppinglistByID.shoppingListName}");

            Console.WriteLine("ITEMS:\n");
            foreach(var i in shoppinglistByID.Items)
            {
                Console.WriteLine($"{i.Name}, {i.Amount}, {i.Type}");

            }
            
            shoppinglistByID.shoppingListName = "This Works very well :3";

            var d = tester.PostShoppingList(shoppinglistByID).Result;
            

            var shoppinglistByIDTESTER = new ShoppingListDTO()
            {
                shoppingListName = "TESTER",
                Items = null,
                shoppingListGroupName = "Karls Gruppe",
                shoppingListGroupID = 1

            };

            //d = tester.PostShoppingList(shoppinglistByIDTESTER).Result;
            var shoppinglistByIDTESTER1 = tester.GetShoppingListByShoppinglistId(6).Result;

            //Console.WriteLine($"{shoppinglistByIDTESTER.shoppingListID}, {shoppinglistByIDTESTER.shoppingListGroupName}, {shoppinglistByIDTESTER.shoppingListName}");
            d= tester.DeleteShoppingList(shoppinglistByIDTESTER1).Result;



            //Console.WriteLine($"{Shoppinglistforid9.shoppingListID}, {Shoppinglistforid9.shoppingListGroupName}, {Shoppinglistforid9.shoppingListName}");
        }

    }
}
