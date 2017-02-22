using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MemberTrack.Data;

namespace MemberTrack.DbUtil.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170216222741_RemoveDocumentTables")]
    partial class RemoveDocumentTables
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

                    b.Property<bool>("HasElementaryKids");

                    b.Property<bool>("HasHighSchoolKids");

                    b.Property<bool>("HasInfantKids");

                    b.Property<bool>("HasJuniorHighKids");

                    b.Property<bool>("HasToddlerKids");

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
