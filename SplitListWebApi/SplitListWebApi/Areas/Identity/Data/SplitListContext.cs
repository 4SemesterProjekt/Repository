using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.Pantry;
using ApiFormat.Recipe;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SplitListWebApi.Areas.Identity.Data
{
   public partial class SplitListContext : IdentityDbContext<UserModel>
    {
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<PantryModel> Pantries { get; set; }
        public DbSet<ShoppingListModel> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<ItemModel> Items { get; set; }
        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<RecipeItem> RecipeItems { get; set; }
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

            modelBuilder.Entity<UserModel>().Property(p => p.ModelId).UseIdentityColumn()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            //Setup for User-Group Many-to-Many
            modelBuilder.Entity<UserGroup>().HasKey(ug => new {ug.GroupModelModelID, ug.UserId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.UserModel)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug =>  ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.GroupModel)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupModelModelID);

            //Setup for ShoppingList-Item Many-to-Many
            modelBuilder.Entity<ShoppingListItem>().HasKey(sli => new {sli.ShoppingListModelID, sli.ItemModelID});
            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.ShoppingListModel)
                .WithMany(sl => sl.ShoppingListItems)
                .HasForeignKey(sli => sli.ShoppingListModelID);

            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.ItemModel)
                .WithMany(i => i.ShoppingListItems)
                .HasForeignKey(sli => sli.ItemModelID);
            
            //Setup for Pantry-Item Many-to-Many
            modelBuilder.Entity<PantryItem>().HasKey(pi => new {pi.PantryModelID, pi.ItemModelID});
            modelBuilder.Entity<PantryItem>()
                .HasOne(pi => pi.PantryModel)
                .WithMany(p => p.PantryItems)
                .HasForeignKey(pi => pi.PantryModelID);

            modelBuilder.Entity<PantryItem>()
                .HasOne(pi => pi.ItemModel)
                .WithMany(i => i.PantryItems)
                .HasForeignKey(pi => pi.ItemModelID);
            
            //Setup for Item-Recipe Many-to-Many
            modelBuilder.Entity<RecipeItem>().HasKey(ir => new {ir.ItemModelID, ir.RecipeModelID});
            modelBuilder.Entity<RecipeItem>()
                .HasOne(ir => ir.ItemModel)
                .WithMany(i => i.RecipeItems)
                .HasForeignKey(ir => ir.ItemModelID);

            modelBuilder.Entity<RecipeItem>()
                .HasOne(ir => ir.RecipeModel)
                .WithMany(r => r.RecipeItems)
                .HasForeignKey(ir => ir.RecipeModelID);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
