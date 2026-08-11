using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericComparisons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComparisonAlternatives",
                columns: table => new
                {
                    ComparisonAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryCode = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonAlternatives", x => x.ComparisonAlternativeId);
                    table.ForeignKey(
                        name: "FK_ComparisonAlternatives_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonScores",
                columns: table => new
                {
                    ComparisonScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComparisonAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    AppliedMaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonScores", x => x.ComparisonScoreId);
                    table.ForeignKey(
                        name: "FK_ComparisonScores_ComparisonAlternatives_ComparisonAlternativeId",
                        column: x => x.ComparisonAlternativeId,
                        principalTable: "ComparisonAlternatives",
                        principalColumn: "ComparisonAlternativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonFactors",
                columns: table => new
                {
                    ComparisonFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComparisonScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(8,4)", precision: 8, scale: 4, nullable: true),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonFactors", x => x.ComparisonFactorId);
                    table.ForeignKey(
                        name: "FK_ComparisonFactors_ComparisonScores_ComparisonScoreId",
                        column: x => x.ComparisonScoreId,
                        principalTable: "ComparisonScores",
                        principalColumn: "ComparisonScoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonAlternatives_AnalysisId_Position",
                table: "ComparisonAlternatives",
                columns: new[] { "AnalysisId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonAlternatives_CategoryCode",
                table: "ComparisonAlternatives",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonFactors_ComparisonScoreId_FactorCode",
                table: "ComparisonFactors",
                columns: new[] { "ComparisonScoreId", "FactorCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonScores_ComparisonAlternativeId",
                table: "ComparisonScores",
                column: "ComparisonAlternativeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComparisonFactors");

            migrationBuilder.DropTable(
                name: "ComparisonScores");

            migrationBuilder.DropTable(
                name: "ComparisonAlternatives");
        }
    }
}
