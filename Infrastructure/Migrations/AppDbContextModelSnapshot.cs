﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BackendOlimpiadaIsto.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BackendOlimpiadaIsto.domain.Entities.PetPrompt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Prompt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PetPrompts");
                });

            modelBuilder.Entity("BackendOlimpiadaIsto.domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("QuestionPrompt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BackendOlimpiadaIsto.domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastAnswerdQuestionStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LastAnsweredQuestionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackendOlimpiadaIsto.domain.Entities.Question", b =>
                {
                    b.OwnsOne("domain.ValueObjects.AnswerList", "Answers", b1 =>
                        {
                            b1.Property<Guid>("QuestionId")
                                .HasColumnType("uuid");

                            b1.PrimitiveCollection<List<string>>("Answers")
                                .IsRequired()
                                .HasColumnType("text[]");

                            b1.Property<int>("CorrectAnswerIndex")
                                .HasColumnType("integer");

                            b1.HasKey("QuestionId");

                            b1.ToTable("Questions");

                            b1.WithOwner()
                                .HasForeignKey("QuestionId");
                        });

                    b.Navigation("Answers")
                        .IsRequired();
                });

            modelBuilder.Entity("BackendOlimpiadaIsto.domain.Entities.User", b =>
                {
                    b.OwnsMany("domain.ValueObjects.AnsweredQuestion", "AnsweredQuestions", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.PrimitiveCollection<List<int>>("Attempts")
                                .IsRequired()
                                .HasColumnType("integer[]");

                            b1.Property<DateTime?>("FinishedDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<Guid>("QuestionId")
                                .HasColumnType("uuid");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("AnsweredQuestion");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("AnsweredQuestions");
                });
#pragma warning restore 612, 618
        }
    }
}
