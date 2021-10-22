﻿// <auto-generated />
using Api.Databases.Schools;
using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Api.Databases.Schools.Migrations
{
    [DbContext(typeof(SchoolsDbContext))]
    [Migration("20190620102718_AddSsoTokens")]
    partial class AddSsoTokens
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Api.Entities.Schools.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AccessAllSessions");

                    b.Property<int>("Course");

                    b.Property<bool>("FirstSessionWithDiagnosis");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<string>("LanguageKey")
                        .IsRequired();

                    b.Property<int>("LimitCourse");

                    b.Property<int>("LimitSession");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("SchoolId")
                        .IsRequired();

                    b.Property<string>("SubjectKey")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasName("Index_Group_UniqueKey");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("Api.Entities.Schools.SsoIdentity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("IdToken");

                    b.HasKey("Id");

                    b.ToTable("SsoIdentity");
                });

            modelBuilder.Entity("Api.Entities.Schools.StudentAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ActivityContentBlockId");

                    b.Property<int>("ActivityDifficulty");

                    b.Property<int>("ActivitySession");

                    b.Property<int>("Course");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<float>("Grade");

                    b.Property<bool>("IsSentToTkReports");

                    b.Property<string>("LanguageKey")
                        .HasColumnType("VARCHAR(8)")
                        .HasMaxLength(8);

                    b.Property<int>("Level");

                    b.Property<int>("Session");

                    b.Property<int>("Stage");

                    b.Property<string>("SubjectKey")
                        .HasColumnType("VARCHAR(8)")
                        .HasMaxLength(8);

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("UserName", "SubjectKey", "LanguageKey")
                        .HasName("Index_StudentAnswer_UserNameSubjectLang");

                    b.ToTable("StudentAnswer");
                });

            modelBuilder.Entity("Api.Entities.Schools.StudentGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessNumber");

                    b.Property<Guid>("GroupId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId", "AccessNumber")
                        .IsUnique()
                        .HasName("Index_StudentGroup_UniqueAccessNumber");

                    b.HasIndex("GroupId", "UserName")
                        .IsUnique()
                        .HasName("Index_StudentGroup_UniqueStudentGroup");

                    b.ToTable("StudentGroup");
                });

            modelBuilder.Entity("Api.Entities.Schools.StudentProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Course");

                    b.Property<int>("DiagnosisTestState");

                    b.Property<string>("LanguageKey");

                    b.Property<int>("Session");

                    b.Property<string>("SubjectKey");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName", "SubjectKey", "LanguageKey")
                        .IsUnique()
                        .HasName("Index_StudentProgress_UniqueSubject")
                        .HasFilter("[UserName] IS NOT NULL AND [SubjectKey] IS NOT NULL AND [LanguageKey] IS NOT NULL");

                    b.ToTable("StudentProgress");
                });

            modelBuilder.Entity("Api.Entities.Schools.StudentGroup", b =>
                {
                    b.HasOne("Api.Entities.Schools.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
