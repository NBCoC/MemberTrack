using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class RemoveChildrenInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildrenInfo");

            migrationBuilder.AddColumn<bool>(
                name: "HasElementaryKids",
                table: "Person",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasHighSchoolKids",
                table: "Person",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInfantKids",
                table: "Person",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasJuniorHighKids",
                table: "Person",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasToddlerKids",
                table: "Person",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasElementaryKids",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HasHighSchoolKids",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HasInfantKids",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HasJuniorHighKids",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HasToddlerKids",
                table: "Person");

            migrationBuilder.CreateTable(
                name: "ChildrenInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AgeGroup = table.Column<int>(nullable: false),
                    PersonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildrenInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildrenInfo_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenInfo_PersonId",
                table: "ChildrenInfo",
                column: "PersonId");
        }
    }
}
