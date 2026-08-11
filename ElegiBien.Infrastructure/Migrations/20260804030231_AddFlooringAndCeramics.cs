using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlooringAndCeramics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlooringCalculationResults",
                columns: table => new
                {
                    FlooringCalculationResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    WastePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    WasteAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RequiredAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    RequiresProfessionalReview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlooringCalculationResults", x => x.FlooringCalculationResultId);
                    table.ForeignKey(
                        name: "FK_FlooringCalculationResults_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlooringInputs",
                columns: table => new
                {
                    FlooringInputId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    WidthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    InstallationPattern = table.Column<int>(type: "int", nullable: false),
                    WastePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlooringInputs", x => x.FlooringInputId);
                    table.ForeignKey(
                        name: "FK_FlooringInputs_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlooringProductAlternatives",
                columns: table => new
                {
                    FlooringProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CoverageSquareMetersPerBox = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    PricePerBox = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UseResistance = table.Column<int>(type: "int", nullable: false),
                    ReplacementEase = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlooringProductAlternatives", x => x.FlooringProductAlternativeId);
                    table.ForeignKey(
                        name: "FK_FlooringProductAlternatives_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlooringProductScores",
                columns: table => new
                {
                    FlooringProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlooringProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    CoverageStatus = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    BoxesRequired = table.Column<int>(type: "int", nullable: false),
                    RequiredAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    PurchasedAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExcessAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExcessPercentage = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlooringProductScores", x => x.FlooringProductScoreId);
                    table.ForeignKey(
                        name: "FK_FlooringProductScores_FlooringProductAlternatives_FlooringProductAlternativeId",
                        column: x => x.FlooringProductAlternativeId,
                        principalTable: "FlooringProductAlternatives",
                        principalColumn: "FlooringProductAlternativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlooringScoreFactors",
                columns: table => new
                {
                    FlooringScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlooringProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlooringScoreFactors", x => x.FlooringScoreFactorId);
                    table.ForeignKey(
                        name: "FK_FlooringScoreFactors_FlooringProductScores_FlooringProductScoreId",
                        column: x => x.FlooringProductScoreId,
                        principalTable: "FlooringProductScores",
                        principalColumn: "FlooringProductScoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlooringCalculationResults_AnalysisId",
                table: "FlooringCalculationResults",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlooringInputs_AnalysisId",
                table: "FlooringInputs",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlooringProductAlternatives_AnalysisId",
                table: "FlooringProductAlternatives",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_FlooringProductScores_FlooringProductAlternativeId",
                table: "FlooringProductScores",
                column: "FlooringProductAlternativeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlooringScoreFactors_FlooringProductScoreId",
                table: "FlooringScoreFactors",
                column: "FlooringProductScoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlooringCalculationResults");

            migrationBuilder.DropTable(
                name: "FlooringInputs");

            migrationBuilder.DropTable(
                name: "FlooringScoreFactors");

            migrationBuilder.DropTable(
                name: "FlooringProductScores");

            migrationBuilder.DropTable(
                name: "FlooringProductAlternatives");
        }
    }
}
