using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class GroupVisitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitType",
                table: "Visit");

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "VisitorCheckListItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VisitorCheckListItem",
                maxLength: 300,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_VisitorCheckListItem_Description",
                table: "VisitorCheckListItem",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_Date",
                table: "Visit",
                column: "Date",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitorCheckListItem_Description",
                table: "VisitorCheckListItem");

            migrationBuilder.DropIndex(
                name: "IX_Visit_Date",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "VisitorCheckListItem");

            migrationBuilder.AddColumn<int>(
                name: "VisitType",
                table: "Visit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "VisitorCheckListItem",
                maxLength: 300,
                nullable: true);
        }
    }
}
