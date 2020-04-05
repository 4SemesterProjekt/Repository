﻿using System;
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


            var shoppinglistByIDTESTER = new ShoppingListDTO()
            {
                shoppingListName = "TESTERer",
                Items = null,
                shoppingListGroupName = "Karls Gruppe",
                shoppingListGroupID = 1

            };

            var NewShoppinglist = SerializerShoppingList.PostShoppingList(shoppinglistByIDTESTER).Result;
            
           
            Console.WriteLine($"{NewShoppinglist.shoppingListID}, {NewShoppinglist.shoppingListGroupName}, {NewShoppinglist.shoppingListName}");
            



            //Console.WriteLine($"{Shoppinglistforid9.shoppingListID}, {Shoppinglistforid9.shoppingListGroupName}, {Shoppinglistforid9.shoppingListName}");
        }

    }
}