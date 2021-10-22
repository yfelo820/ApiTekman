﻿// <auto-generated />
using System;
using Api.Databases.Content;
using Api.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Databases.Content.Migrations
{
    [DbContext(typeof(ContentDbContext))]
    [Migration("20201111071913_CreateAudioPlayerTypeProperty")]
    partial class CreateAudioPlayerTypeProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Api.Entities.Content.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ContentBlockId");

                    b.Property<Guid>("CourseId");

                    b.Property<string>("Description")
                        .HasMaxLength(150);

                    b.Property<int>("Difficulty");

                    b.Property<bool>("IsDiagnosis");

                    b.Property<bool>("IsTimeDependant");

                    b.Property<Guid>("LanguageId");

                    b.Property<int>("QuestionCount");

                    b.Property<int>("Session");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(50);

                    b.Property<int>("Stage");

                    b.Property<Guid>("SubjectId");

                    b.Property<int>("WordCount");

                    b.HasKey("Id");

                    b.HasIndex("ContentBlockId");

                    b.HasIndex("CourseId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("Api.Entities.Content.ContentBlock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<Guid>("LanguageId");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<Guid>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("Order", "LanguageId", "SubjectId")
                        .IsUnique()
                        .HasName("Index_ContentBlock_UniqueOrderForLanguageSubject");

                    b.ToTable("ContentBlock");
                });

            modelBuilder.Entity("Api.Entities.Content.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("Api.Entities.Content.DragDrop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ItemDragId");

                    b.Property<Guid>("ItemDropId");

                    b.Property<int>("MultipleDragResult");

                    b.HasKey("Id");

                    b.HasIndex("ItemDropId");

                    b.HasIndex("ItemDragId", "ItemDropId")
                        .IsUnique()
                        .HasName("Index_DragDrop_UniqueDragDrop");

                    b.ToTable("DragDrop");
                });

            modelBuilder.Entity("Api.Entities.Content.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("AudioPlayerType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue((byte)0);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("Height");

                    b.Property<byte>("MediaType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue((byte)0);

                    b.Property<string>("MediaUrl");

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

            modelBuilder.Entity("Api.Entities.Content.Language", b =>
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

            modelBuilder.Entity("Api.Entities.Content.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Api.Entities.Content.Scene", b =>
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

            modelBuilder.Entity("Api.Entities.Content.Style", b =>
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

            modelBuilder.Entity("Api.Entities.Content.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DifficultyCount");

                    b.Property<string>("Key");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SessionCount");

                    b.HasKey("Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Api.Entities.Content.Transition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Transition");
                });

            modelBuilder.Entity("Api.Entities.Content.TransitionProperty", b =>
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

            modelBuilder.Entity("Api.Entities.Content.ItemDrag", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");

                    b.Property<bool>("IsMultiple");

                    b.ToTable("ItemDrag");

                    b.HasDiscriminator().HasValue("ItemDrag");
                });

            modelBuilder.Entity("Api.Entities.Content.ItemDraw", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");

                    b.Property<string>("LineColor");

                    b.Property<int>("LineWidth");

                    b.ToTable("ItemDraw");

                    b.HasDiscriminator().HasValue("ItemDraw");
                });

            modelBuilder.Entity("Api.Entities.Content.ItemDrop", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");


                    b.ToTable("ItemDrop");

                    b.HasDiscriminator().HasValue("ItemDrop");
                });

            modelBuilder.Entity("Api.Entities.Content.ItemInput", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");

                    b.Property<bool>("IsIgnoringAccents");

                    b.Property<bool>("IsIgnoringCaps");

                    b.Property<bool>("IsNumber");

                    b.Property<string>("Solution");

                    b.ToTable("ItemInput");

                    b.HasDiscriminator().HasValue("ItemInput");
                });

            modelBuilder.Entity("Api.Entities.Content.ItemSelect", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");

                    b.Property<bool>("IsCorrect");

                    b.ToTable("ItemSelect");

                    b.HasDiscriminator().HasValue("ItemSelect");
                });

            modelBuilder.Entity("Api.Entities.Content.ItemStatic", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Item");


                    b.ToTable("ItemStatic");

                    b.HasDiscriminator().HasValue("ItemStatic");
                });

            modelBuilder.Entity("Api.Entities.Content.Achievement", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Scene");

                    b.Property<string>("Image");

                    b.Property<Guid>("LanguageId");

                    b.Property<string>("Name");

                    b.Property<int>("Session");

                    b.Property<Guid>("SubjectId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Achievement");

                    b.HasDiscriminator().HasValue("Achievement");
                });

            modelBuilder.Entity("Api.Entities.Content.Exercise", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Scene");

                    b.Property<Guid>("ActivityId");

                    b.Property<int>("Order");

                    b.HasIndex("ActivityId");

                    b.ToTable("Exercise");

                    b.HasDiscriminator().HasValue("Exercise");
                });

            modelBuilder.Entity("Api.Entities.Content.Feedback", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Scene");

                    b.Property<Guid>("LanguageId")
                        .HasColumnName("Feedback_LanguageId");

                    b.Property<int>("Score");

                    b.Property<Guid>("SubjectId")
                        .HasColumnName("Feedback_SubjectId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Feedback");

                    b.HasDiscriminator().HasValue("Feedback");
                });

            modelBuilder.Entity("Api.Entities.Content.Template", b =>
                {
                    b.HasBaseType("Api.Entities.Content.Scene");

                    b.Property<Guid>("LanguageId")
                        .HasColumnName("Template_LanguageId");

                    b.Property<string>("Name")
                        .HasColumnName("Template_Name");

                    b.Property<Guid>("SubjectId")
                        .HasColumnName("Template_SubjectId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Template");

                    b.HasDiscriminator().HasValue("Template");
                });

            modelBuilder.Entity("Api.Entities.Content.Activity", b =>
                {
                    b.HasOne("Api.Entities.Content.ContentBlock", "ContentBlock")
                        .WithMany()
                        .HasForeignKey("ContentBlockId");

                    b.HasOne("Api.Entities.Content.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.ContentBlock", b =>
                {
                    b.HasOne("Api.Entities.Content.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.DragDrop", b =>
                {
                    b.HasOne("Api.Entities.Content.ItemDrag")
                        .WithMany("DropAnswers")
                        .HasForeignKey("ItemDragId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.ItemDrop")
                        .WithMany("DragAnswers")
                        .HasForeignKey("ItemDropId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Item", b =>
                {
                    b.HasOne("Api.Entities.Content.Scene")
                        .WithMany("Items")
                        .HasForeignKey("SceneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Scene", b =>
                {
                    b.HasOne("Api.Entities.Content.Transition", "Transition")
                        .WithMany()
                        .HasForeignKey("TransitionId");
                });

            modelBuilder.Entity("Api.Entities.Content.Style", b =>
                {
                    b.HasOne("Api.Entities.Content.Item")
                        .WithOne("Style")
                        .HasForeignKey("Api.Entities.Content.Style", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.TransitionProperty", b =>
                {
                    b.HasOne("Api.Entities.Content.Transition")
                        .WithMany("TransitionProperties")
                        .HasForeignKey("TransitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Achievement", b =>
                {
                    b.HasOne("Api.Entities.Content.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Exercise", b =>
                {
                    b.HasOne("Api.Entities.Content.Activity", "Activity")
                        .WithMany("Exercises")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Feedback", b =>
                {
                    b.HasOne("Api.Entities.Content.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.Entities.Content.Template", b =>
                {
                    b.HasOne("Api.Entities.Content.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Entities.Content.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
