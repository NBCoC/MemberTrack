using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AgeGroup = table.Column<int>(nullable: false),
                    BaptismDate = table.Column<DateTimeOffset>(nullable: true),
                    ContactNumber = table.Column<string>(maxLength: 15, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(maxLength: 75, nullable: false),
                    FirstVisitDate = table.Column<DateTimeOffset>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    HasElementaryKids = table.Column<bool>(nullable: false),
                    HasHighSchoolKids = table.Column<bool>(nullable: false),
                    HasInfantKids = table.Column<bool>(nullable: false),
                    HasJuniorHighKids = table.Column<bool>(nullable: false),
                    HasToddlerKids = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(maxLength: 75, nullable: false),
                    MembershipDate = table.Column<DateTimeOffset>(nullable: true),
                    MiddleName = table.Column<string>(maxLength: 75, nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonCheckListItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckListItemType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCheckListItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(maxLength: 256, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    PersonId = table.Column<long>(nullable: false),
                    City = table.Column<string>(maxLength: 150, nullable: false),
                    State = table.Column<int>(nullable: false),
                    Street = table.Column<string>(maxLength: 150, nullable: false),
                    ZipCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Address_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonCheckList",
                columns: table => new
                {
                    PersonId = table.Column<long>(nullable: false),
                    PersonCheckListItemId = table.Column<long>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCheckList", x => new { x.PersonId, x.PersonCheckListItemId });
                    table.ForeignKey(
                        name: "FK_PersonCheckList_PersonCheckListItem_PersonCheckListItemId",
                        column: x => x.PersonCheckListItemId,
                        principalTable: "PersonCheckListItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonCheckList_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_PersonId",
                table: "Address",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_Email",
                table: "Person",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonCheckList_PersonCheckListItemId",
                table: "PersonCheckList",
                column: "PersonCheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCheckList_PersonId",
                table: "PersonCheckList",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCheckListItem_Description",
                table: "PersonCheckListItem",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PersonCheckList");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PersonCheckListItem");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
