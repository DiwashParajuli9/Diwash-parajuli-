using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JournalApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Emoji = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsCustom = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrimaryMoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondaryMood1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondaryMood2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_PrimaryMoodId",
                        column: x => x.PrimaryMoodId,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_SecondaryMood1Id",
                        column: x => x.SecondaryMood1Id,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Moods_SecondaryMood2Id",
                        column: x => x.SecondaryMood2Id,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryTag",
                columns: table => new
                {
                    JournalEntriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryTag", x => new { x.JournalEntriesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_JournalEntryTag_JournalEntries_JournalEntriesId",
                        column: x => x.JournalEntriesId,
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalEntryTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Moods",
                columns: new[] { "Id", "Category", "Emoji", "Name" },
                values: new object[,]
                {
                    { 1, 0, "😊", "Happy" },
                    { 2, 0, "🤗", "Excited" },
                    { 3, 0, "😌", "Relaxed" },
                    { 4, 0, "🙏", "Grateful" },
                    { 5, 0, "💪", "Confident" },
                    { 6, 1, "😐", "Calm" },
                    { 7, 1, "🤔", "Thoughtful" },
                    { 8, 1, "🧐", "Curious" },
                    { 9, 1, "😌", "Nostalgic" },
                    { 10, 1, "😑", "Bored" },
                    { 11, 2, "😔", "Sad" },
                    { 12, 2, "😠", "Angry" },
                    { 13, 2, "😰", "Stressed" },
                    { 14, 2, "😢", "Lonely" },
                    { 15, 2, "😟", "Anxious" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "IsCustom", "Name" },
                values: new object[,]
                {
                    { 1, false, "Work" },
                    { 2, false, "Career" },
                    { 3, false, "Studies" },
                    { 4, false, "Family" },
                    { 5, false, "Friends" },
                    { 6, false, "Relationships" },
                    { 7, false, "Health" },
                    { 8, false, "Fitness" },
                    { 9, false, "Personal Growth" },
                    { 10, false, "Self-care" },
                    { 11, false, "Hobbies" },
                    { 12, false, "Travel" },
                    { 13, false, "Nature" },
                    { 14, false, "Finance" },
                    { 15, false, "Spirituality" },
                    { 16, false, "Birthday" },
                    { 17, false, "Holiday" },
                    { 18, false, "Vacation" },
                    { 19, false, "Celebration" },
                    { 20, false, "Exercise" },
                    { 21, false, "Reading" },
                    { 22, false, "Writing" },
                    { 23, false, "Cooking" },
                    { 24, false, "Meditation" },
                    { 25, false, "Yoga" },
                    { 26, false, "Music" },
                    { 27, false, "Shopping" },
                    { 28, false, "Parenting" },
                    { 29, false, "Projects" },
                    { 30, false, "Planning" },
                    { 31, false, "Reflection" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_PrimaryMoodId",
                table: "JournalEntries",
                column: "PrimaryMoodId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SecondaryMood1Id",
                table: "JournalEntries",
                column: "SecondaryMood1Id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_SecondaryMood2Id",
                table: "JournalEntries",
                column: "SecondaryMood2Id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryTag_TagsId",
                table: "JournalEntryTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntryTag");

            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Moods");
        }
    }
}
