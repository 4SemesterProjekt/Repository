﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SplitListWebApi.Migrations
{
    [DbContext(typeof(SplitListWebApi.Models.DbContext))]
    [Migration("20200309105850_ConvertedToContext")]
    partial class ConvertedToContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SplitListWebApi.Models.Group", b =>
                {
                    b.Property<int>("GroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OwnerID")
                        .HasColumnType("int");

                    b.HasKey("GroupID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SplitListWebApi.Models.Item", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SplitListWebApi.Models.ItemRecipe", b =>
                {
                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<int>("RecipeID")
                        .HasColumnType("int");

                    b.HasKey("ItemID", "RecipeID");

                    b.HasIndex("RecipeID");

                    b.ToTable("ItemRecipe");
                });

            modelBuilder.Entity("SplitListWebApi.Models.Pantry", b =>
                {
                    b.Property<int>("PantryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GroupID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PantryID");

                    b.HasIndex("GroupID");

                    b.ToTable("Pantries");
                });

            modelBuilder.Entity("SplitListWebApi.Models.PantryItem", b =>
                {
                    b.Property<int>("PantryID")
                        .HasColumnType("int");

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("PantryID", "ItemID");

                    b.HasIndex("ItemID");

                    b.ToTable("PantryItem");
                });

            modelBuilder.Entity("SplitListWebApi.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("SplitListWebApi.Models.ShoppingList", b =>
                {
                    b.Property<int>("ShoppingListID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GroupID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShoppingListID");

                    b.HasIndex("GroupID");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("SplitListWebApi.Models.ShoppingListItem", b =>
                {
                    b.Property<int>("ShoppingListID")
                        .HasColumnType("int");

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("ShoppingListID", "ItemID");

                    b.HasIndex("ItemID");

                    b.ToTable("ShoppingListItem");
                });

            modelBuilder.Entity("SplitListWebApi.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SplitListWebApi.Models.UserGroup", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "GroupID");

                    b.HasIndex("GroupID");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("SplitListWebApi.Models.Group", b =>
                {
                    b.HasOne("SplitListWebApi.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID");
                });

            modelBuilder.Entity("SplitListWebApi.Models.ItemRecipe", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Item", "item")
                        .WithMany("ItemRecipes")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitListWebApi.Models.Recipe", "Recipe")
                        .WithMany("ItemRecipes")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitListWebApi.Models.Pantry", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Group", "Group")
                        .WithMany("Pantries")
                        .HasForeignKey("GroupID");
                });

            modelBuilder.Entity("SplitListWebApi.Models.PantryItem", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Item", "Item")
                        .WithMany("PantryItems")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitListWebApi.Models.Pantry", "Pantry")
                        .WithMany("PantryItems")
                        .HasForeignKey("PantryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitListWebApi.Models.ShoppingList", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Group", "Group")
                        .WithMany("ShoppingLists")
                        .HasForeignKey("GroupID");
                });

            modelBuilder.Entity("SplitListWebApi.Models.ShoppingListItem", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Item", "Item")
                        .WithMany("ShoppingListItems")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitListWebApi.Models.ShoppingList", "ShoppingList")
                        .WithMany("ShoppingListItems")
                        .HasForeignKey("ShoppingListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitListWebApi.Models.UserGroup", b =>
                {
                    b.HasOne("SplitListWebApi.Models.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitListWebApi.Models.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
