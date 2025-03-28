﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notadesigner.Approximato.Data;

#nullable disable

namespace Notadesigner.Approximato.Data.Migrations
{
    [DbContext(typeof(ApproximatoDbContext))]
    [Migration("20241022140449_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Notadesigner.Approximato.Data.Models.Pomodoro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pomodoros");
                });

            modelBuilder.Entity("Notadesigner.Approximato.Data.Models.Transition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<int>("PomodoroId")
                        .HasMaxLength(16)
                        .HasColumnType("INTEGER");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PomodoroId");

                    b.ToTable("Transitions");
                });

            modelBuilder.Entity("Notadesigner.Approximato.Data.Models.Transition", b =>
                {
                    b.HasOne("Notadesigner.Approximato.Data.Models.Pomodoro", "Pomodoro")
                        .WithMany("Transitions")
                        .HasForeignKey("PomodoroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pomodoro");
                });

            modelBuilder.Entity("Notadesigner.Approximato.Data.Models.Pomodoro", b =>
                {
                    b.Navigation("Transitions");
                });
#pragma warning restore 612, 618
        }
    }
}
