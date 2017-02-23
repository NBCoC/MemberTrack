﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MemberTrack.Data;

namespace MemberTrack.DbUtil.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20161230171811_CreateDB")]
    partial class CreateDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MemberTrack.Data.Entities.Address", b =>
                {
                    b.Property<long>("PersonId");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 150);

                    b.Property<int>("State");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 150);

                    b.Property<int>("ZipCode");

                    b.HasKey("PersonId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.ChildrenInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgeGroup");

                    b.Property<long>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("ChildrenInfo");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Document", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 350);

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 5);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<long>("Size");

                    b.HasKey("Id");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.DocumentData", b =>
                {
                    b.Property<long>("DocumentId");

                    b.Property<byte[]>("Data")
                        .IsRequired();

                    b.HasKey("DocumentId");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("DocumentData");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.DocumentTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DocumentId");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 75);

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("DocumentTag");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgeGroup");

                    b.Property<DateTimeOffset?>("BaptismDate");

                    b.Property<string>("ContactNumber")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 75);

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 75);

                    b.Property<DateTimeOffset?>("MembershipDate");

                    b.Property<string>("MiddleName")
                        .HasAnnotation("MaxLength", 75);

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Person");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.PersonCheckList", b =>
                {
                    b.Property<long>("PersonId");

                    b.Property<long>("PersonCheckListItemId");

                    b.Property<DateTimeOffset>("Date");

                    b.Property<string>("Note")
                        .HasAnnotation("MaxLength", 500);

                    b.HasKey("PersonId", "PersonCheckListItemId");

                    b.HasIndex("PersonCheckListItemId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonCheckList");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.PersonCheckListItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CheckListItemType");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("PersonCheckListItem");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Address", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Person", "Person")
                        .WithOne("Address")
                        .HasForeignKey("MemberTrack.Data.Entities.Address", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.ChildrenInfo", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Person", "Person")
                        .WithMany("ChildrenInfos")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.DocumentData", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Document", "Document")
                        .WithOne("DocumentData")
                        .HasForeignKey("MemberTrack.Data.Entities.DocumentData", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.DocumentTag", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Document", "Document")
                        .WithMany("DocumentTags")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.PersonCheckList", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.PersonCheckListItem", "PersonCheckListItem")
                        .WithMany("CheckLists")
                        .HasForeignKey("PersonCheckListItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemberTrack.Data.Entities.Person", "Person")
                        .WithMany("CheckLists")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}