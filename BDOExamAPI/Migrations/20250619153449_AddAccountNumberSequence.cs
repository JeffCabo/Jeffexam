using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDOExamAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountNumberSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountNumberSequences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenceDate = table.Column<DateTime>(type: "datetime2", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNumberSequences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountNumberSequences_SequenceDate",
                table: "AccountNumberSequences",
                column: "SequenceDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountNumberSequences");
        }
    }
}
