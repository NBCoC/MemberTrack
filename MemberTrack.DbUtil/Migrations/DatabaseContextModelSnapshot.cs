using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MemberTrack.Data;

namespace MemberTrack.DbUtil.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MemberTrack.Data.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgeGroup");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<DateTimeOffset?>("FirstVisitDate");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 150);

                    b.Property<DateTimeOffset?>("MembershipDate");

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

                    b.Property<int>("SortOrder");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("PersonCheckListItem");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Answer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<long>("QuestionId");

                    b.Property<long?>("TopicId");

                    b.Property<int>("TopicWeight");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("TopicId");

                    b.ToTable("QuizAnswer");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.PersonAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AnswerId");

                    b.Property<long>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("PersonId");

                    b.ToTable("QuizPersonAnswer");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllowMultipleAnswers");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 300);

                    b.Property<long>("QuizId");

                    b.Property<bool>("RandomizeAnswers");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestion");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Quiz", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Instructions");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("RandomizeQuestions");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name")
                        .HasName("AlternateKey_Name");

                    b.ToTable("Quiz");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.SupportingScripture", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("ScriptureReference")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<long>("TopicId");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("QuizSupportingScripture");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Topic", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<long>("TopicCategoryId");

                    b.HasKey("Id");

                    b.HasIndex("TopicCategoryId");

                    b.ToTable("QuizTopic");
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.TopicCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("QuizTopicCategory");
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

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Answer", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Quizzes.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemberTrack.Data.Entities.Quizzes.Topic", "Topic")
                        .WithMany("Answers")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.PersonAnswer", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Quizzes.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemberTrack.Data.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Question", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Quizzes.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.SupportingScripture", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Quizzes.Topic", "Topic")
                        .WithMany("SupportingScriptures")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemberTrack.Data.Entities.Quizzes.Topic", b =>
                {
                    b.HasOne("MemberTrack.Data.Entities.Quizzes.TopicCategory", "TopicCategory")
                        .WithMany("Topics")
                        .HasForeignKey("TopicCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
