﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using back_1._2.Data;

#nullable disable

namespace back_1._2.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("back_1._2.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("city");

                    b.Property<string>("House")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("house");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("street");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("back_1._2.Models.AddressInUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("AddressInUsers");
                });

            modelBuilder.Entity("back_1._2.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("back_1._2.Models.ItemsInUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("isFavourite")
                        .HasColumnType("bit");

                    b.Property<bool>("isInCart")
                        .HasColumnType("bit");

                    b.Property<int>("itemId")
                        .HasColumnType("int");

                    b.Property<int>("itemInCartNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("itemId");

                    b.ToTable("ItemsInUser");
                });

            modelBuilder.Entity("back_1._2.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<int>("Sum")
                        .HasColumnType("int")
                        .HasColumnName("sum");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("back_1._2.Models.AddressInUser", b =>
                {
                    b.HasOne("back_1._2.Models.Address", "Address")
                        .WithMany("AddressInUsers")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("back_1._2.Models.User", "User")
                        .WithMany("AddressInUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("back_1._2.Models.ItemsInUser", b =>
                {
                    b.HasOne("back_1._2.Models.User", "User")
                        .WithMany("ItemsInUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("back_1._2.Models.Item", "item")
                        .WithMany("ItemsInUser")
                        .HasForeignKey("itemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("item");
                });

            modelBuilder.Entity("back_1._2.Models.Address", b =>
                {
                    b.Navigation("AddressInUsers");
                });

            modelBuilder.Entity("back_1._2.Models.Item", b =>
                {
                    b.Navigation("ItemsInUser");
                });

            modelBuilder.Entity("back_1._2.Models.User", b =>
                {
                    b.Navigation("AddressInUsers");

                    b.Navigation("ItemsInUser");
                });
#pragma warning restore 612, 618
        }
    }
}
