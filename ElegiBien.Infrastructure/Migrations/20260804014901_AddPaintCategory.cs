using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaintCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaintCalculationResults",
                columns: table => new
                {
                    PaintCalculationResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WallAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CeilingAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DeductedAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    NetAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AdjustedAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ReferenceCoverageSquareMetersPerLiter = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    ReferenceLiters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    RequiresProfessionalReview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintCalculationResults", x => x.PaintCalculationResultId);
                    table.ForeignKey(
                        name: "FK_PaintCalculationResults_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintInputs",
                columns: table => new
                {
                    PaintInputId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    WidthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    HeightMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IncludeCeiling = table.Column<bool>(type: "bit", nullable: false),
                    DoorCount = table.Column<int>(type: "int", nullable: false),
                    WindowCount = table.Column<int>(type: "int", nullable: false),
                    CoatCount = table.Column<int>(type: "int", nullable: false),
                    SurfaceCondition = table.Column<int>(type: "int", nullable: false),
                    WastePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintInputs", x => x.PaintInputId);
                    table.ForeignKey(
                        name: "FK_PaintInputs_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintProductAlternatives",
                columns: table => new
                {
                    PaintProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContainerLiters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    PricePerContainer = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CoverageSquareMetersPerLiterPerCoat = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Washability = table.Column<int>(type: "int", nullable: false),
                    DryingHours = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintProductAlternatives", x => x.PaintProductAlternativeId);
                    table.ForeignKey(
                        name: "FK_PaintProductAlternatives_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintProductScores",
                columns: table => new
                {
                    PaintProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    CoverageStatus = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    ContainersRequired = table.Column<int>(type: "int", nullable: false),
                    LitersRequired = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    LitersPurchased = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintProductScores", x => x.PaintProductScoreId);
                    table.ForeignKey(
                        name: "FK_PaintProductScores_PaintProductAlternatives_PaintProductAlternativeId",
                        column: x => x.PaintProductAlternativeId,
                        principalTable: "PaintProductAlternatives",
                        principalColumn: "PaintProductAlternativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaintScoreFactors",
                columns: table => new
                {
                    PaintScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintScoreFactors", x => x.PaintScoreFactorId);
                    table.ForeignKey(
                        name: "FK_PaintScoreFactors_PaintProductScores_PaintProductScoreId",
                        column: x => x.PaintProductScoreId,
                        principalTable: "PaintProductScores",
                        principalColumn: "PaintProductScoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaintCalculationResults_AnalysisId",
                table: "PaintCalculationResults",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintInputs_AnalysisId",
                table: "PaintInputs",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductAlternatives_AnalysisId",
                table: "PaintProductAlternatives",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintProductScores_PaintProductAlternativeId",
                table: "PaintProductScores",
                column: "PaintProductAlternativeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaintScoreFactors_PaintProductScoreId",
                table: "PaintScoreFactors",
                column: "PaintProductScoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaintCalculationResults");

            migrationBuilder.DropTable(
                name: "PaintInputs");

            migrationBuilder.DropTable(
                name: "PaintScoreFactors");

            migrationBuilder.DropTable(
                name: "PaintProductScores");

            migrationBuilder.DropTable(
                name: "PaintProductAlternatives");
        }
    }
}
