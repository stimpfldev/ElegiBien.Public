using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElegiBien.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "MethodologyVersions",
                columns: table => new
                {
                    MethodologyVersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodologyVersions", x => x.MethodologyVersionId);
                    table.ForeignKey(
                        name: "FK_MethodologyVersions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    MethodologyVersionId = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.AnalysisId);
                    table.ForeignKey(
                        name: "FK_Analyses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Analyses_MethodologyVersions_MethodologyVersionId",
                        column: x => x.MethodologyVersionId,
                        principalTable: "MethodologyVersions",
                        principalColumn: "MethodologyVersionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AirConditioningInputs",
                columns: table => new
                {
                    AirConditioningInputId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    WidthMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    HeightMeters = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IsHeightAssumed = table.Column<bool>(type: "bit", nullable: false),
                    PeopleCount = table.Column<int>(type: "int", nullable: false),
                    SunExposure = table.Column<int>(type: "int", nullable: false),
                    ClimateZone = table.Column<int>(type: "int", nullable: false),
                    InsulationLevel = table.Column<int>(type: "int", nullable: false),
                    WindowExposure = table.Column<int>(type: "int", nullable: false),
                    IsOpenToAnotherSpace = table.Column<bool>(type: "bit", nullable: false),
                    HasHighHeatEquipment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirConditioningInputs", x => x.AirConditioningInputId);
                    table.ForeignKey(
                        name: "FK_AirConditioningInputs_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalyticsEvents",
                columns: table => new
                {
                    AnalyticsEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: true),
                    OccurredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsEvents", x => x.AnalyticsEventId);
                    table.ForeignKey(
                        name: "FK_AnalyticsEvents_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId");
                    table.ForeignKey(
                        name: "FK_AnalyticsEvents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsentRecords",
                columns: table => new
                {
                    ConsentRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsentType = table.Column<int>(type: "int", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    LegalVersion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RecordedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentRecords", x => x.ConsentRecordId);
                    table.ForeignKey(
                        name: "FK_ConsentRecords_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimensioningResults",
                columns: table => new
                {
                    DimensioningResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolumeCubicMeters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    BaseFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AdjustmentFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    EstimatedFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RecommendedMinimumFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RecommendedMaximumFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    IdealFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    RequiresProfessionalReview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensioningResults", x => x.DimensioningResultId);
                    table.ForeignKey(
                        name: "FK_DimensioningResults_Analyses_AnalysisId",
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
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CapacityFrigories = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Technology = table.Column<int>(type: "int", nullable: false),
                    NominalConsumptionWatts = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    WarrantyMonths = table.Column<int>(type: "int", nullable: false),
                    ReferenceUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                name: "SharedResults",
                columns: table => new
                {
                    SharedResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccessCount = table.Column<int>(type: "int", nullable: false),
                    LastAccessedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedResults", x => x.SharedResultId);
                    table.ForeignKey(
                        name: "FK_SharedResults_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "AnalysisId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductScores",
                columns: table => new
                {
                    ProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAlternativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    AppliedMaximumScore = table.Column<int>(type: "int", nullable: true),
                    CapacityStatus = table.Column<int>(type: "int", nullable: false),
                    ConfidenceLevel = table.Column<int>(type: "int", nullable: false),
                    IsEligible = table.Column<bool>(type: "bit", nullable: false)
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
                name: "ScoreFactors",
                columns: table => new
                {
                    ScoreFactorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactorType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    MaximumScore = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
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
                name: "IX_AirConditioningInputs_AnalysisId",
                table: "AirConditioningInputs",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CategoryId_CreatedAtUtc",
                table: "Analyses",
                columns: new[] { "CategoryId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CreatedAtUtc",
                table: "Analyses",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_MethodologyVersionId",
                table: "Analyses",
                column: "MethodologyVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_AnalysisId",
                table: "AnalyticsEvents",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_CategoryId_OccurredAtUtc",
                table: "AnalyticsEvents",
                columns: new[] { "CategoryId", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_EventType_OccurredAtUtc",
                table: "AnalyticsEvents",
                columns: new[] { "EventType", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_OccurredAtUtc",
                table: "AnalyticsEvents",
                column: "OccurredAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_AnalysisId_ConsentType",
                table: "ConsentRecords",
                columns: new[] { "AnalysisId", "ConsentType" });

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_RecordedAtUtc",
                table: "ConsentRecords",
                column: "RecordedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DimensioningResults_AnalysisId",
                table: "DimensioningResults",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MethodologyVersions_CategoryId_Version",
                table: "MethodologyVersions",
                columns: new[] { "CategoryId", "Version" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_SharedResults_AnalysisId",
                table: "SharedResults",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedResults_IsActive_ExpiresAtUtc",
                table: "SharedResults",
                columns: new[] { "IsActive", "ExpiresAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_SharedResults_PublicToken",
                table: "SharedResults",
                column: "PublicToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirConditioningInputs");

            migrationBuilder.DropTable(
                name: "AnalyticsEvents");

            migrationBuilder.DropTable(
                name: "ConsentRecords");

            migrationBuilder.DropTable(
                name: "DimensioningResults");

            migrationBuilder.DropTable(
                name: "ScoreFactors");

            migrationBuilder.DropTable(
                name: "SharedResults");

            migrationBuilder.DropTable(
                name: "ProductScores");

            migrationBuilder.DropTable(
                name: "ProductAlternatives");

            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "MethodologyVersions");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
