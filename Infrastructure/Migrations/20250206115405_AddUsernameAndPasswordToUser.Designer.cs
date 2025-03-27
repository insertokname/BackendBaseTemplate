﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BackendBaseTemplate.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250206115405_AddUsernameAndPasswordToUser")]
    partial class AddUsernameAndPasswordToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BackendBaseTemplate.domain.Entities.PetPrompt", b =>
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

            modelBuilder.Entity("BackendBaseTemplate.domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("QuestionPrompt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BackendBaseTemplate.domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
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

            modelBuilder.Entity("BackendBaseTemplate.domain.Entities.Question", b =>
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
#pragma warning restore 612, 618
        }
    }
}
