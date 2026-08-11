using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyComparisonTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlooringScoreFactors");

            migrationBuilder.DropTable(
                name: "HeatingScoreFactors");

            migrationBuilder.DropTable(
                name: "PaintScoreFactors");

            migrationBuilder.DropTable(
                name: "ScoreFactors");

            migrationBuilder.DropTable(
                name: "FlooringProductScores");

            migrationBuilder.DropTable(
                name: "HeatingProductScores");

            migrationBuilder.DropTable(
                name: "PaintProductScores");

            migrationBuilder.DropTable(
                name: "ProductScores");

            migrationBuilder.DropTable(
                name: "FlooringProductAlternatives");

            migrationBuilder.DropTable(
                name: "HeatingProductAlternatives");

            migrationBuilder.DropTable(
                name: "PaintProductAlternatives");

            migrationBuilder.DropTable(
                name: "ProductAlternatives");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlooringProductAlternatives",
                columns: table => new
                {
                    FlooringProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoverageSquareMetersPerBox = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PricePerBox = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReplacementEase = table.Column<int>(type: "int", nullable: false),
                    UseResistance = table.Column<int>(type: "int", nullable: false)
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
                name: "HeatingProductAlternatives",
                columns: table => new
                {
                    HeatingProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EfficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    EstimatedHourlyCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HeatingCapacityWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SafetyLevel = table.Column<int>(type: "int", nullable: false),
                    SystemType = table.Column<int>(type: "int", nullable: false)
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
                name: "PaintProductAlternatives",
                columns: table => new
                {
                    PaintProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContainerLiters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    CoverageSquareMetersPerLiterPerCoat = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    DryingHours = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PricePerContainer = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Washability = table.Column<int>(type: "int", nullable: false)
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
                name: "ProductAlternatives",
                columns: table => new
                {
                    ProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CapacityFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NominalConsumptionWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReferenceUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Technology = table.Column<int>(type: "int", nullable: false),
                    WarrantyMonths = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAlternatives", x => x.ProductAlternativeId);
                    table.ForeignKey(
                        name: "FK_ProductAlternatives_Analyses_AnalysisId",
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
                    BoxesRequired = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    CoverageStatus = table.Column<int>(type: "int", nullable: false),
                    ExcessAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExcessPercentage = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    PurchasedAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RequiredAreaSquareMeters = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
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
                name: "HeatingProductScores",
                columns: table => new
                {
                    HeatingProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeatingProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppliedMaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    CapacityStatus = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
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
                name: "PaintProductScores",
                columns: table => new
                {
                    PaintProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    ContainersRequired = table.Column<int>(type: "int", nullable: false),
                    CoverageStatus = table.Column<int>(type: "int", nullable: false),
                    LitersPurchased = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    LitersRequired = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
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
                name: "ProductScores",
                columns: table => new
                {
                    ProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppliedMaximumScore = table.Column<int>(type: "int", nullable: true),
                    CapacityStatus = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductScores", x => x.ProductScoreId);
                    table.ForeignKey(
                        name: "FK_ProductScores_ProductAlternatives_ProductAlternativeId",
                        column: x => x.ProductAlternativeId,
                        principalTable: "ProductAlternatives",
                        principalColumn: "ProductAlternativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlooringScoreFactors",
                columns: table => new
                {
                    FlooringScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlooringProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "HeatingScoreFactors",
                columns: table => new
                {
                    HeatingScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeatingProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PaintScoreFactors",
                columns: table => new
                {
                    PaintScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaintProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ScoreFactors",
                columns: table => new
                {
                    ScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreFactors", x => x.ScoreFactorId);
                    table.ForeignKey(
                        name: "FK_ScoreFactors_ProductScores_ProductScoreId",
                        column: x => x.ProductScoreId,
                        principalTable: "ProductScores",
                        principalColumn: "ProductScoreId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductAlternatives_AnalysisId",
                table: "ProductAlternatives",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductScores_ProductAlternativeId",
                table: "ProductScores",
                column: "ProductAlternativeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreFactors_ProductScoreId",
                table: "ScoreFactors",
                column: "ProductScoreId");
        }
    }
}
