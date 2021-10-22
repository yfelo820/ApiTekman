﻿// <auto-generated />
using Api.Databases.Content;
using Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Api.Database.Migrations
{
    [DbContext(typeof(ContentDbContext))]
    [Migration("20180314164223_ContentBlockActivityRel")]
    partial class ContentBlockActivityRel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Api.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ContentBlockId");

                    b.Property<Guid>("CourseId");

                    b.Property<string>("Description");

                    b.Property<int>("Difficulty");

                    b.Property<bool>("IsTimeDependant");

                    b.Property<Guid>("LanguageId");

                    b.Property<int>("Session");

                    b.Property<int>("Stage");

                    b.Property<Guid>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("ContentBlockId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("CourseId", "SubjectId", "Stage", "Difficulty", "LanguageId", "Session")
                        .IsUnique()
                        .HasName("Index_Activity_CourseSubjectStageDifficulty");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("Api.Entities.ContentBlock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<Guid>("LanguageId");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("Order", "LanguageId")
                        .IsUnique()
                        .HasName("Index_ContentBlock_UniqueOrderForLanguage");

                    b.ToTable("ContentBlock");
                });

            modelBuilder.Entity("Api.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("Api.Entities.DragDrop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ItemDragId");

                    b.Property<Guid>("ItemDropId");

                    b.HasKey("Id");

                    b.HasIndex("ItemDropId");

                    b.HasIndex("ItemDragId", "ItemDropId")
                        .IsUnique()
                        .HasName("Index_DragDrop_UniqueDragDrop");

                    b.ToTable("DragDrop");
                });

            modelBuilder.Entity("Api.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("Height");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<int>("Rotation");

                    b.Property<Guid>("SceneId");

                    b.Property<string>("Text");

                    b.Property<int>("Width");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.Property<int>("ZIndex");

                    b.HasKey("Id");

                    b.HasIndex("SceneId");

                    b.ToTable("Item");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Item");
                });

            modelBuilder.Entity("Api.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Api.Entities.Scene", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackgroundImage");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Thumbnail");

                    b.Property<Guid?>("TransitionId");

                    b.HasKey("Id");

                    b.HasIndex("TransitionId");

                    b.ToTable("Scene");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Scene");
                });

            modelBuilder.Entity("Api.Entities.Style", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackgroundColor");

                    b.Property<string>("BorderColor");

                    b.Property<int>("BorderRadius");

                    b.Property<int>("BorderStyle");

                    b.Property<int>("BorderWidth");

                    b.Property<bool>("IsShadowInset");

                    b.Property<Guid>("ItemId");

                    b.Property<int>("ShadowBlur");

                    b.Property<string>("ShadowColor");

                    b.Property<int>("ShadowHorizontalOffset");

                    b.Property<float>("ShadowOpacity");

                    b.Property<int>("ShadowVerticalOffset");

                    b.Property<string>("TextShadow");

                    b.HasKey("Id");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("Style");
                });

            modelBuilder.Entity("Api.Entities.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DifficultyCount");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("StageCount");

                    b.HasKey("Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Api.Entities.Transition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Transition");
                });

            modelBuilder.Entity("Api.Entities.TransitionProperty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemState");

                    b.Property<int>("ItemType");

                    b.Property<int>("Property");

                    b.Property<Guid>("TransitionId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TransitionId");

                    b.HasIndex("ItemState", "ItemType", "Property", "TransitionId")
                        .IsUnique()
                        .HasName("Index_TransitionProperty_UniqueTransitionProp");

                    b.ToTable("TransitionProperty");
                });

            modelBuilder.Entity("Api.Entities.ItemDrag", b =>
                {
                    b.HasBaseType("Api.Entities.Item");


                    b.ToTable("ItemDrag");

                    b.HasDiscriminator().HasValue("ItemDrag");
                });

            modelBuilder.Entity("Api.Entities.ItemDrop", b =>
                {
                    b.HasBaseType("Api.Entities.Item");


                    b.ToTable("ItemDrop");

                    b.HasDiscriminator().HasValue("ItemDrop");
                });

            modelBuilder.Entity("Api.Entities.ItemInput", b =>
                {
                    b.HasBaseType("Api.Entities.Item");

                    b.Property<bool>("IsIgnoringAccents");

                    b.Property<bool>("IsIgnoringCaps");

                    b.Property<bool>("IsNumber");

                    b.Property<string>("Solution");

                    b.ToTable("ItemInput");

                    b.HasDiscriminator().HasValue("ItemInput");
                });

            modelBuilder.Entity("Api.Entities.ItemSelect", b =>
                {
                    b.HasBaseType("Api.Entities.Item");

                    b.Property<bool>("IsCorrect");

                    b.ToTable("ItemSelect");

                    b.HasDiscriminator().HasValue("ItemSelect");
                });

            modelBuilder.Entity("Api.Entities.ItemStatic", b =>
                {
                    b.HasBaseType("Api.Entities.Item");


                    b.ToTable("ItemStatic");

                    b.HasDiscriminator().HasValue("ItemStatic");
                });

            modelBuilder.Entity("Api.Entities.Exercise", b =>
                {
                    b.HasBaseType("Api.Entities.Scene");

                    b.Property<Guid>("ActivityId");

                    b.Property<int>("Order");

                    b.HasIndex("ActivityId");

                    b.ToTable("Exercise");

                    b.HasDiscriminator().HasValue("Exercise");
                });

            modelBuilder.Entity("Api.Entities.Template", b =>
                {
                    b.HasBaseType("Api.Entities.Scene");

                    b.Property<Guid>("LanguageId");

                    b.Property<string>("Name");

                    b.Property<Guid>("SubjectId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Template");

                    b.HasDiscriminator().HasValue("Template");
                });

            modelBuilder.Entity("Api.Entities.Activity", b =>
                {
                    b.HasOne("Api.Entities.ContentBlock", "ContentBlock")
                        .WithMany()
                        .HasForeignKey("ContentBlockId");

                    b.HasOne("Api.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.ContentBlock", b =>
                {
                    b.HasOne("Api.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.DragDrop", b =>
                {
                    b.HasOne("Api.Entities.ItemDrag")
                        .WithMany("DropAnswers")
                        .HasForeignKey("ItemDragId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.ItemDrop")
                        .WithMany("DragAnswers")
                        .HasForeignKey("ItemDropId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Item", b =>
                {
                    b.HasOne("Api.Entities.Scene")
                        .WithMany("Items")
                        .HasForeignKey("SceneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Scene", b =>
                {
                    b.HasOne("Api.Entities.Transition", "Transition")
                        .WithMany()
                        .HasForeignKey("TransitionId");
                });

            modelBuilder.Entity("Api.Entities.Style", b =>
                {
                    b.HasOne("Api.Entities.Item")
                        .WithOne("Style")
                        .HasForeignKey("Api.Entities.Style", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.TransitionProperty", b =>
                {
                    b.HasOne("Api.Entities.Transition")
                        .WithMany("TransitionProperties")
                        .HasForeignKey("TransitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Exercise", b =>
                {
                    b.HasOne("Api.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Template", b =>
                {
                    b.HasOne("Api.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
