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

            /* ____________________________________TEST af funktionalitet for shoppinglist__________________________*/

            //Console.WriteLine("Hello world");
            //var Shoppinglistforid1 = SerializerShoppingList.GetShoppingListByGroupId(1).Result;

            //foreach (var i in Shoppinglistforid1)
            //{
            //    Console.WriteLine($"{i.shoppingListGroupID}, {i.shoppingListID}, {i.shoppingListName}");
            //}
            //Console.WriteLine("SHOPPINGLIST BY ID:\n");
            //var shoppinglistByID = SerializerShoppingList.GetShoppingListByShoppinglistId(1).Result;

            //Console.WriteLine($"{shoppinglistByID.shoppingListID}, {shoppinglistByID.shoppingListGroupName}, {shoppinglistByID.shoppingListName}");

            //Console.WriteLine("ITEMS:\n");
            //foreach (var i in shoppinglistByID.Items)
            //{
            //    Console.WriteLine($"{i.Name}, {i.Amount}, {i.Type}");

            //}

            //shoppinglistByID.shoppingListName = "This Works very well :3";

            //var d = SerializerShoppingList.PostShoppingList(shoppinglistByID).Result;


            //var testerforItem = SerializerShoppingList.GetShoppingListByShoppinglistId(1).Result;
            //testerforItem.Items.Add(new ItemDTO()
            //{
            //    ItemID = 0,
            //    Name = "Gifler",
            //    Amount = 3


            //});

            //var NewShoppinglist = SerializerShoppingList.PostShoppingList(testerforItem).Result;


            //Console.WriteLine($"{NewShoppinglist.shoppingListID}, {NewShoppinglist.shoppingListGroupName}, {NewShoppinglist.shoppingListName}");


            ////Console.WriteLine($"{Shoppinglistforid9.shoppingListID}, {Shoppinglistforid9.shoppingListGroupName}, {Shoppinglistforid9.shoppingListName}");


            /* ____________________________________TEST af funktionalitet for Pantry__________________________*/
            //PantryDTO MarkusPantry = new PantryDTO
            //{
            //    GroupID = 2,
            //    Items = null,
            //    Name = "Nikolaj's køleskab",
            //    GroupName = "Markus Gruppe"
            //};

            //var pantryMarkus1 = SerializerPantry.PostPantry(MarkusPantry).Result;
            //var pantryOfMarkus = SerializerPantry.GetPantryByGroupId(2).Result;
            //Console.WriteLine($"{pantryOfMarkus.ID}, {pantryOfMarkus.Name}, {pantryOfMarkus.GroupName}");
            //var d = SerializerPantry.DeletePantry(pantryOfMarkus);
            //var pantryOfNikolaj = SerializerPantry.GetPantryByGroupId(1).Result;

            //pantryOfNikolaj.Items.Add(new ItemDTO()
            //{

            //    Name = "Gifler",
            //    Amount = 3


            //});

            //var pantryOfNikolaj1 = SerializerPantry.PostPantry(pantryOfNikolaj).Result;
            //Console.WriteLine($"{pantryOfNikolaj1.ID}, {pantryOfNikolaj1.Name}, {pantryOfNikolaj1.GroupName}");




            /* ____________________________________TEST af funktionalitet for Groups__________________________*/

            var NikoGroup = SerializerGroups.GetGroupByGroupId(1).Result;
            Console.WriteLine($"{NikoGroup.GroupID}, {NikoGroup.OwnerID}, {NikoGroup.ShoppingLists[0].shoppingListName}");

            var niko = SerializerGroups.GetGroupOwnerByGroupId(1).Result;
            Console.WriteLine($"{niko.Name}");

            GroupDTO Studiegruppe = new GroupDTO
            {
                Name = "StudieGruppe",
                OwnerID = niko.Id

            };
            var StudieGruppe1 = SerializerGroups.GetGroupByGroupId(3).Result;
            Console.WriteLine($"{StudieGruppe1.GroupID}, {StudieGruppe1.OwnerID}");

            var d = SerializerGroups.DeleteGroup(StudieGruppe1).Result;
        }

    }
}