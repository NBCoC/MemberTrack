using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class RenameVisitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorCheckList");

            migrationBuilder.DropTable(
                name: "VisitorCheckListItem");

            migrationBuilder.CreateTable(
                name: "VisitCheckListItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    Group = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitCheckListItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitCheckList",
                columns: table => new
                {
                    VisitorId = table.Column<long>(nullable: false),
                    VisitCheckListItemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitCheckList", x => new { x.VisitorId, x.VisitCheckListItemId });
                    table.ForeignKey(
                        name: "FK_VisitCheckList_VisitCheckListItem_VisitCheckListItemId",
                        column: x => x.VisitCheckListItemId,
                        principalTable: "VisitCheckListItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitCheckList_Visit_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visit",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitCheckList_VisitCheckListItemId",
                table: "VisitCheckList",
                column: "VisitCheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitCheckList_VisitorId",
                table: "VisitCheckList",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitCheckListItem_Description",
                table: "VisitCheckListItem",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitCheckList");

            migrationBuilder.DropTable(
                name: "VisitCheckListItem");

            migrationBuilder.CreateTable(
                name: "VisitorCheckListItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    Group = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorCheckListItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitorCheckList",
                columns: table => new
                {
                    VisitorId = table.Column<long>(nullable: false),
                    VisitorCheckListItemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorCheckList", x => new { x.VisitorId, x.VisitorCheckListItemId });
                    table.ForeignKey(
                        name: "FK_VisitorCheckList_VisitorCheckListItem_VisitorCheckListItemId",
                        column: x => x.VisitorCheckListItemId,
                        principalTable: "VisitorCheckListItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitorCheckList_Visit_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visit",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorCheckList_VisitorCheckListItemId",
                table: "VisitorCheckList",
                column: "VisitorCheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorCheckList_VisitorId",
                table: "VisitorCheckList",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorCheckListItem_Description",
                table: "VisitorCheckListItem",
                column: "Description",
                unique: true);
        }
    }
}
