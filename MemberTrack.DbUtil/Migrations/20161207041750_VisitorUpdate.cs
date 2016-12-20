using System;
using System.Collections.Generic;
using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class VisitorUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitType",
                table: "VisitorCheckListItem");

            migrationBuilder.AddColumn<int>(
                name: "VisitType",
                table: "Visit",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitType",
                table: "Visit");

            migrationBuilder.AddColumn<int>(
                name: "VisitType",
                table: "VisitorCheckListItem",
                nullable: false,
                defaultValue: 0);
        }
    }
}
