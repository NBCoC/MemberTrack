﻿using System;
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
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    FirstVisitDate = table.Column<DateTimeOffset>(nullable: true),
                    FullName = table.Column<string>(maxLength: 150, nullable: false),
                    MembershipDate = table.Column<DateTimeOffset>(nullable: true),
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
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    RandomizeQuestions = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.UniqueConstraint("AlternateKey_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "QuizTopicCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizTopicCategory", x => x.Id);
                    table.UniqueConstraint("AK_QuizTopicCategory_Name", x => x.Name);
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

            migrationBuilder.CreateTable(
                name: "QuizQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllowMultipleAnswers = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: false),
                    QuizId = table.Column<long>(nullable: false),
                    RandomizeAnswers = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizTopic",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TopicCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizTopic_QuizTopicCategory_TopicCategoryId",
                        column: x => x.TopicCategoryId,
                        principalTable: "QuizTopicCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    QuestionId = table.Column<long>(nullable: false),
                    TopicId = table.Column<long>(nullable: true),
                    TopicWeight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAnswer_QuizQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuizQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAnswer_QuizTopic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "QuizTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QuizSupportingScripture",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    ScriptureReference = table.Column<string>(maxLength: 50, nullable: false),
                    TopicId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSupportingScripture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizSupportingScripture_QuizTopic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "QuizTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_QuizAnswer_QuestionId",
                table: "QuizAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswer_TopicId",
                table: "QuizAnswer",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizPersonAnswer_AnswerId",
                table: "QuizPersonAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizPersonAnswer_PersonId",
                table: "QuizPersonAnswer",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_QuizId",
                table: "QuizQuestion",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizSupportingScripture_TopicId",
                table: "QuizSupportingScripture",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizTopic_TopicCategoryId",
                table: "QuizTopic",
                column: "TopicCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonCheckList");

            migrationBuilder.DropTable(
                name: "QuizPersonAnswer");

            migrationBuilder.DropTable(
                name: "QuizSupportingScripture");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PersonCheckListItem");

            migrationBuilder.DropTable(
                name: "QuizAnswer");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "QuizQuestion");

            migrationBuilder.DropTable(
                name: "QuizTopic");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "QuizTopicCategory");
        }
    }
}
