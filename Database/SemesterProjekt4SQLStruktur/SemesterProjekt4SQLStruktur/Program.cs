using System;
using Microsoft.EntityFrameworkCore;

namespace SemesterProjekt4SQLStruktur
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DbContext())
            {
                var karl = new User(){Name="Karl Jørgen"};
                var karlsGruppe = new Group(){Name="Karls Gruppe", Owner = karl};
                var karlsShoppingList = new ShoppingList(){Name = "Karls Gruppes Shoppinglist", Group = karlsGruppe};
                var KarlGruppeShadow = new UserGroup(){Group = karlsGruppe, User = karl};

                db.Add(karlsShoppingList);
                db.SaveChanges();
            }
        }
    }
}