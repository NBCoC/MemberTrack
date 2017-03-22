using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.DbUtil.Migrations
{
    public partial class PersonAnswerTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizUserAnswer");

            migrationBuilder.CreateTable(
                name: "QuizPersonAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<long>(nullable: false),
                    PersonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizPersonAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizPersonAnswer_QuizAnswer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuizAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizPersonAnswer_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizPersonAnswer_AnswerId",
                table: "QuizPersonAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizPersonAnswer_PersonId",
                table: "QuizPersonAnswer",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizPersonAnswer");

            migrationBuilder.CreateTable(
                name: "QuizUserAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizUserAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizUserAnswer_QuizAnswer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuizAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizUserAnswer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizUserAnswer_AnswerId",
                table: "QuizUserAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizUserAnswer_UserId",
                table: "QuizUserAnswer",
                column: "UserId");
        }
    }
}
