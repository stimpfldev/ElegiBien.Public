using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHeatingComparison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatingCalculationResults",
                columns: table => new
                {
                    HeatingCalculationResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfaceSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    VolumeCubicMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    BasePowerWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AdjustmentPowerWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    EstimatedPowerWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RecommendedMinimumWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RecommendedMaximumWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    IdealPowerWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    IdealPowerKcalPerHour = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    RequiresProfessionalReview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingCalculationResults", x => x.HeatingCalculationResultId);
                    table.ForeignKey(
                        name: "FK_HeatingCalculationResults_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatingInputs",
                columns: table => new
                {
                    HeatingInputId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    WidthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    HeightMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IsHeightAssumed = table.Column<bool>(type: "bit", nullable: false),
                    ClimateZone = table.Column<int>(type: "int", nullable: false),
                    InsulationLevel = table.Column<int>(type: "int", nullable: false),
                    ExteriorWallsCount = table.Column<int>(type: "int", nullable: false),
                    WindowExposure = table.Column<int>(type: "int", nullable: false),
                    IsOpenToAnotherSpace = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingInputs", x => x.HeatingInputId);
                    table.ForeignKey(
                        name: "FK_HeatingInputs_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatingProductAlternatives",
                columns: table => new
                {
                    HeatingProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SystemType = table.Column<int>(type: "int", nullable: false),
                    HeatingCapacityWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedHourlyCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EfficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    SafetyLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingProductAlternatives", x => x.HeatingProductAlternativeId);
                    table.ForeignKey(
                        name: "FK_HeatingProductAlternatives_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatingProductScores",
                columns: table => new
                {
                    HeatingProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeatingProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    CapacityStatus = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    AppliedMaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingProductScores", x => x.HeatingProductScoreId);
                    table.ForeignKey(
                        name: "FK_HeatingProductScores_HeatingProductAlternatives_HeatingProductAlternativeId",
                        column: x => x.HeatingProductAlternativeId,
                        principalTable: "HeatingProductAlternatives",
                        principalColumn: "HeatingProductAlternativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeatingScoreFactors",
                columns: table => new
                {
                    HeatingScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeatingProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingScoreFactors", x => x.HeatingScoreFactorId);
                    table.ForeignKey(
                        name: "FK_HeatingScoreFactors_HeatingProductScores_HeatingProductScoreId",
                        column: x => x.HeatingProductScoreId,
                        principalTable: "HeatingProductScores",
                        principalColumn: "HeatingProductScoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeatingCalculationResults_AnalysisId",
                table: "HeatingCalculationResults",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeatingInputs_AnalysisId",
                table: "HeatingInputs",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeatingProductAlternatives_AnalysisId",
                table: "HeatingProductAlternatives",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_HeatingProductScores_HeatingProductAlternativeId",
                table: "HeatingProductScores",
                column: "HeatingProductAlternativeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeatingScoreFactors_HeatingProductScoreId",
                table: "HeatingScoreFactors",
                column: "HeatingProductScoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatingCalculationResults");

            migrationBuilder.DropTable(
                name: "HeatingInputs");

            migrationBuilder.DropTable(
                name: "HeatingScoreFactors");

            migrationBuilder.DropTable(
                name: "HeatingProductScores");

            migrationBuilder.DropTable(
                name: "HeatingProductAlternatives");
        }
    }
}
