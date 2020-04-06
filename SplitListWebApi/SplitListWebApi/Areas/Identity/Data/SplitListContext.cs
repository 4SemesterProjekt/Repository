using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data.Models;
using SplitListWebApi.Models;

namespace SplitListWebApi.Areas.Identity.Data
{
   public partial class SplitListContext : IdentityDbContext<User>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Pantry> Pantries { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<PantryItem> PantryItems { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public SplitListContext()
        {
        }

        public SplitListContext(DbContextOptions<SplitListContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Setup for User-Group Many-to-Many
            modelBuilder.Entity<UserGroup>().HasKey(ug => new {ug.Id, ug.GroupID});
            
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.Id);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupID);

            //Setup for ShoppingList-Item Many-to-Many
            modelBuilder.Entity<ShoppingListItem>().HasKey(sli => new {sli.ShoppingListID, sli.ItemID});
            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.ShoppingList)
                .WithMany(sl => sl.ShoppingListItems)
                .HasForeignKey(sli => sli.ShoppingListID);

            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.Item)
                .WithMany(i => i.ShoppingListItems)
                .HasForeignKey(sli => sli.ItemID);
            
            //Setup for Pantry-Item Many-to-Many
            modelBuilder.Entity<PantryItem>().HasKey(pi => new {pi.PantryID, pi.ItemID});
            modelBuilder.Entity<PantryItem>()
                .HasOne(pi => pi.Pantry)
                .WithMany(p => p.PantryItems)
                .HasForeignKey(pi => pi.PantryID);

            modelBuilder.Entity<PantryItem>()
                .HasOne(pi => pi.Item)
                .WithMany(i => i.PantryItems)
                .HasForeignKey(pi => pi.ItemID);
            
            //Setup for Item-Recipe Many-to-Many
            modelBuilder.Entity<ItemRecipe>().HasKey(ir => new {ir.ItemID, ir.RecipeID});
            modelBuilder.Entity<ItemRecipe>()
                .HasOne(ir => ir.item)
                .WithMany(i => i.ItemRecipes)
                .HasForeignKey(ir => ir.ItemID);

            modelBuilder.Entity<ItemRecipe>()
                .HasOne(ir => ir.Recipe)
                .WithMany(r => r.ItemRecipes)
                .HasForeignKey(ir => ir.RecipeID);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
