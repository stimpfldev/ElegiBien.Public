IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [CategoryId] int NOT NULL IDENTITY,
        [Code] int NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Slug] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [MethodologyVersions] (
        [MethodologyVersionId] int NOT NULL IDENTITY,
        [CategoryId] int NOT NULL,
        [Version] nvarchar(30) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [EffectiveFromUtc] datetime2 NOT NULL,
        [EffectiveToUtc] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_MethodologyVersions] PRIMARY KEY ([MethodologyVersionId]),
        CONSTRAINT [FK_MethodologyVersions_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [Analyses] (
        [AnalysisId] uniqueidentifier NOT NULL,
        [CategoryId] int NOT NULL,
        [MethodologyVersionId] int NOT NULL,
        [Mode] int NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [CompletedAtUtc] datetime2 NULL,
        [IsCompleted] bit NOT NULL,
        CONSTRAINT [PK_Analyses] PRIMARY KEY ([AnalysisId]),
        CONSTRAINT [FK_Analyses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Analyses_MethodologyVersions_MethodologyVersionId] FOREIGN KEY ([MethodologyVersionId]) REFERENCES [MethodologyVersions] ([MethodologyVersionId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [AirConditioningInputs] (
        [AirConditioningInputId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [LengthMeters] decimal(8,2) NOT NULL,
        [WidthMeters] decimal(8,2) NOT NULL,
        [HeightMeters] decimal(8,2) NOT NULL,
        [IsHeightAssumed] bit NOT NULL,
        [PeopleCount] int NOT NULL,
        [SunExposure] int NOT NULL,
        [ClimateZone] int NOT NULL,
        [InsulationLevel] int NOT NULL,
        [WindowExposure] int NOT NULL,
        [IsOpenToAnotherSpace] bit NOT NULL,
        [HasHighHeatEquipment] bit NOT NULL,
        CONSTRAINT [PK_AirConditioningInputs] PRIMARY KEY ([AirConditioningInputId]),
        CONSTRAINT [FK_AirConditioningInputs_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [AnalyticsEvents] (
        [AnalyticsEventId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NULL,
        [CategoryId] int NOT NULL,
        [EventType] int NOT NULL,
        [Mode] int NULL,
        [OccurredAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_AnalyticsEvents] PRIMARY KEY ([AnalyticsEventId]),
        CONSTRAINT [FK_AnalyticsEvents_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]),
        CONSTRAINT [FK_AnalyticsEvents_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [ConsentRecords] (
        [ConsentRecordId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [ConsentType] int NOT NULL,
        [IsGranted] bit NOT NULL,
        [LegalVersion] nvarchar(30) NOT NULL,
        [RecordedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_ConsentRecords] PRIMARY KEY ([ConsentRecordId]),
        CONSTRAINT [FK_ConsentRecords_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [DimensioningResults] (
        [DimensioningResultId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [VolumeCubicMeters] decimal(10,2) NOT NULL,
        [BaseFrigories] decimal(12,2) NOT NULL,
        [AdjustmentFrigories] decimal(12,2) NOT NULL,
        [EstimatedFrigories] decimal(12,2) NOT NULL,
        [RecommendedMinimumFrigories] decimal(12,2) NOT NULL,
        [RecommendedMaximumFrigories] decimal(12,2) NOT NULL,
        [IdealFrigories] decimal(12,2) NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [RequiresProfessionalReview] bit NOT NULL,
        CONSTRAINT [PK_DimensioningResults] PRIMARY KEY ([DimensioningResultId]),
        CONSTRAINT [FK_DimensioningResults_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductAlternatives] (
        [ProductAlternativeId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Brand] nvarchar(100) NULL,
        [CapacityFrigories] decimal(12,2) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Technology] int NOT NULL,
        [NominalConsumptionWatts] decimal(12,2) NULL,
        [WarrantyMonths] int NOT NULL,
        [ReferenceUrl] nvarchar(1000) NULL,
        CONSTRAINT [PK_ProductAlternatives] PRIMARY KEY ([ProductAlternativeId]),
        CONSTRAINT [FK_ProductAlternatives_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [SharedResults] (
        [SharedResultId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [PublicToken] nvarchar(100) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [AccessCount] int NOT NULL,
        [LastAccessedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_SharedResults] PRIMARY KEY ([SharedResultId]),
        CONSTRAINT [FK_SharedResults_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductScores] (
        [ProductScoreId] uniqueidentifier NOT NULL,
        [ProductAlternativeId] uniqueidentifier NOT NULL,
        [TotalScore] int NOT NULL,
        [AppliedMaximumScore] int NULL,
        [CapacityStatus] int NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [IsEligible] bit NOT NULL,
        CONSTRAINT [PK_ProductScores] PRIMARY KEY ([ProductScoreId]),
        CONSTRAINT [FK_ProductScores_ProductAlternatives_ProductAlternativeId] FOREIGN KEY ([ProductAlternativeId]) REFERENCES [ProductAlternatives] ([ProductAlternativeId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE TABLE [ScoreFactors] (
        [ScoreFactorId] uniqueidentifier NOT NULL,
        [ProductScoreId] uniqueidentifier NOT NULL,
        [FactorType] int NOT NULL,
        [Score] decimal(6,2) NOT NULL,
        [MaximumScore] decimal(6,2) NOT NULL,
        [Explanation] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_ScoreFactors] PRIMARY KEY ([ScoreFactorId]),
        CONSTRAINT [FK_ScoreFactors_ProductScores_ProductScoreId] FOREIGN KEY ([ProductScoreId]) REFERENCES [ProductScores] ([ProductScoreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_AirConditioningInputs_AnalysisId] ON [AirConditioningInputs] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Analyses_CategoryId_CreatedAtUtc] ON [Analyses] ([CategoryId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Analyses_CreatedAtUtc] ON [Analyses] ([CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Analyses_MethodologyVersionId] ON [Analyses] ([MethodologyVersionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AnalyticsEvents_AnalysisId] ON [AnalyticsEvents] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AnalyticsEvents_CategoryId_OccurredAtUtc] ON [AnalyticsEvents] ([CategoryId], [OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AnalyticsEvents_EventType_OccurredAtUtc] ON [AnalyticsEvents] ([EventType], [OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AnalyticsEvents_OccurredAtUtc] ON [AnalyticsEvents] ([OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categories_Code] ON [Categories] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categories_Slug] ON [Categories] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ConsentRecords_AnalysisId_ConsentType] ON [ConsentRecords] ([AnalysisId], [ConsentType]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ConsentRecords_RecordedAtUtc] ON [ConsentRecords] ([RecordedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DimensioningResults_AnalysisId] ON [DimensioningResults] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MethodologyVersions_CategoryId_Version] ON [MethodologyVersions] ([CategoryId], [Version]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductAlternatives_AnalysisId] ON [ProductAlternatives] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProductScores_ProductAlternativeId] ON [ProductScores] ([ProductAlternativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ScoreFactors_ProductScoreId] ON [ScoreFactors] ([ProductScoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SharedResults_AnalysisId] ON [SharedResults] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SharedResults_IsActive_ExpiresAtUtc] ON [SharedResults] ([IsActive], [ExpiresAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_SharedResults_PublicToken] ON [SharedResults] ([PublicToken]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803015619_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260803015619_InitialCreate', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE TABLE [PaintCalculationResults] (
        [PaintCalculationResultId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [WallAreaSquareMeters] decimal(12,2) NOT NULL,
        [CeilingAreaSquareMeters] decimal(12,2) NOT NULL,
        [DeductedAreaSquareMeters] decimal(12,2) NOT NULL,
        [NetAreaSquareMeters] decimal(12,2) NOT NULL,
        [AdjustedAreaSquareMeters] decimal(12,2) NOT NULL,
        [ReferenceCoverageSquareMetersPerLiter] decimal(8,2) NOT NULL,
        [ReferenceLiters] decimal(10,2) NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [RequiresProfessionalReview] bit NOT NULL,
        CONSTRAINT [PK_PaintCalculationResults] PRIMARY KEY ([PaintCalculationResultId]),
        CONSTRAINT [FK_PaintCalculationResults_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE TABLE [PaintInputs] (
        [PaintInputId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [LengthMeters] decimal(8,2) NOT NULL,
        [WidthMeters] decimal(8,2) NOT NULL,
        [HeightMeters] decimal(8,2) NOT NULL,
        [IncludeCeiling] bit NOT NULL,
        [DoorCount] int NOT NULL,
        [WindowCount] int NOT NULL,
        [CoatCount] int NOT NULL,
        [SurfaceCondition] int NOT NULL,
        [WastePercentage] decimal(5,2) NOT NULL,
        CONSTRAINT [PK_PaintInputs] PRIMARY KEY ([PaintInputId]),
        CONSTRAINT [FK_PaintInputs_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE TABLE [PaintProductAlternatives] (
        [PaintProductAlternativeId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [ContainerLiters] decimal(8,2) NOT NULL,
        [PricePerContainer] decimal(18,2) NOT NULL,
        [CoverageSquareMetersPerLiterPerCoat] decimal(8,2) NOT NULL,
        [Washability] int NOT NULL,
        [DryingHours] decimal(6,2) NULL,
        CONSTRAINT [PK_PaintProductAlternatives] PRIMARY KEY ([PaintProductAlternativeId]),
        CONSTRAINT [FK_PaintProductAlternatives_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE TABLE [PaintProductScores] (
        [PaintProductScoreId] uniqueidentifier NOT NULL,
        [PaintProductAlternativeId] uniqueidentifier NOT NULL,
        [TotalScore] int NOT NULL,
        [CoverageStatus] int NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [ContainersRequired] int NOT NULL,
        [LitersRequired] decimal(10,2) NOT NULL,
        [LitersPurchased] decimal(10,2) NOT NULL,
        [TotalCost] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_PaintProductScores] PRIMARY KEY ([PaintProductScoreId]),
        CONSTRAINT [FK_PaintProductScores_PaintProductAlternatives_PaintProductAlternativeId] FOREIGN KEY ([PaintProductAlternativeId]) REFERENCES [PaintProductAlternatives] ([PaintProductAlternativeId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE TABLE [PaintScoreFactors] (
        [PaintScoreFactorId] uniqueidentifier NOT NULL,
        [PaintProductScoreId] uniqueidentifier NOT NULL,
        [FactorType] int NOT NULL,
        [Score] decimal(8,2) NOT NULL,
        [MaximumScore] decimal(8,2) NOT NULL,
        [Explanation] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_PaintScoreFactors] PRIMARY KEY ([PaintScoreFactorId]),
        CONSTRAINT [FK_PaintScoreFactors_PaintProductScores_PaintProductScoreId] FOREIGN KEY ([PaintProductScoreId]) REFERENCES [PaintProductScores] ([PaintProductScoreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PaintCalculationResults_AnalysisId] ON [PaintCalculationResults] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PaintInputs_AnalysisId] ON [PaintInputs] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE INDEX [IX_PaintProductAlternatives_AnalysisId] ON [PaintProductAlternatives] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PaintProductScores_PaintProductAlternativeId] ON [PaintProductScores] ([PaintProductAlternativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    CREATE INDEX [IX_PaintScoreFactors_PaintProductScoreId] ON [PaintScoreFactors] ([PaintProductScoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804014901_AddPaintCategory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260804014901_AddPaintCategory', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE TABLE [FlooringCalculationResults] (
        [FlooringCalculationResultId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [TotalAreaSquareMeters] decimal(12,2) NOT NULL,
        [WastePercentage] decimal(5,2) NOT NULL,
        [WasteAreaSquareMeters] decimal(12,2) NOT NULL,
        [RequiredAreaSquareMeters] decimal(12,2) NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [RequiresProfessionalReview] bit NOT NULL,
        CONSTRAINT [PK_FlooringCalculationResults] PRIMARY KEY ([FlooringCalculationResultId]),
        CONSTRAINT [FK_FlooringCalculationResults_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE TABLE [FlooringInputs] (
        [FlooringInputId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [LengthMeters] decimal(8,2) NOT NULL,
        [WidthMeters] decimal(8,2) NOT NULL,
        [InstallationPattern] int NOT NULL,
        [WastePercentage] decimal(5,2) NOT NULL,
        CONSTRAINT [PK_FlooringInputs] PRIMARY KEY ([FlooringInputId]),
        CONSTRAINT [FK_FlooringInputs_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE TABLE [FlooringProductAlternatives] (
        [FlooringProductAlternativeId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [CoverageSquareMetersPerBox] decimal(8,2) NOT NULL,
        [PricePerBox] decimal(18,2) NOT NULL,
        [UseResistance] int NOT NULL,
        [ReplacementEase] int NOT NULL,
        CONSTRAINT [PK_FlooringProductAlternatives] PRIMARY KEY ([FlooringProductAlternativeId]),
        CONSTRAINT [FK_FlooringProductAlternatives_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE TABLE [FlooringProductScores] (
        [FlooringProductScoreId] uniqueidentifier NOT NULL,
        [FlooringProductAlternativeId] uniqueidentifier NOT NULL,
        [TotalScore] int NOT NULL,
        [CoverageStatus] int NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [BoxesRequired] int NOT NULL,
        [RequiredAreaSquareMeters] decimal(12,2) NOT NULL,
        [PurchasedAreaSquareMeters] decimal(12,2) NOT NULL,
        [ExcessAreaSquareMeters] decimal(12,2) NOT NULL,
        [ExcessPercentage] decimal(8,2) NOT NULL,
        [TotalCost] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_FlooringProductScores] PRIMARY KEY ([FlooringProductScoreId]),
        CONSTRAINT [FK_FlooringProductScores_FlooringProductAlternatives_FlooringProductAlternativeId] FOREIGN KEY ([FlooringProductAlternativeId]) REFERENCES [FlooringProductAlternatives] ([FlooringProductAlternativeId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE TABLE [FlooringScoreFactors] (
        [FlooringScoreFactorId] uniqueidentifier NOT NULL,
        [FlooringProductScoreId] uniqueidentifier NOT NULL,
        [FactorType] int NOT NULL,
        [Score] decimal(8,2) NOT NULL,
        [MaximumScore] decimal(8,2) NOT NULL,
        [Explanation] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_FlooringScoreFactors] PRIMARY KEY ([FlooringScoreFactorId]),
        CONSTRAINT [FK_FlooringScoreFactors_FlooringProductScores_FlooringProductScoreId] FOREIGN KEY ([FlooringProductScoreId]) REFERENCES [FlooringProductScores] ([FlooringProductScoreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE UNIQUE INDEX [IX_FlooringCalculationResults_AnalysisId] ON [FlooringCalculationResults] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE UNIQUE INDEX [IX_FlooringInputs_AnalysisId] ON [FlooringInputs] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE INDEX [IX_FlooringProductAlternatives_AnalysisId] ON [FlooringProductAlternatives] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE UNIQUE INDEX [IX_FlooringProductScores_FlooringProductAlternativeId] ON [FlooringProductScores] ([FlooringProductAlternativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    CREATE INDEX [IX_FlooringScoreFactors_FlooringProductScoreId] ON [FlooringScoreFactors] ([FlooringProductScoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804030231_AddFlooringAndCeramics'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260804030231_AddFlooringAndCeramics', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804042159_AddHeating'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260804042159_AddHeating', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE TABLE [HeatingCalculationResults] (
        [HeatingCalculationResultId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [SurfaceSquareMeters] decimal(12,2) NOT NULL,
        [VolumeCubicMeters] decimal(12,2) NOT NULL,
        [BasePowerWatts] decimal(12,2) NOT NULL,
        [AdjustmentPowerWatts] decimal(12,2) NOT NULL,
        [EstimatedPowerWatts] decimal(12,2) NOT NULL,
        [RecommendedMinimumWatts] decimal(12,2) NOT NULL,
        [RecommendedMaximumWatts] decimal(12,2) NOT NULL,
        [IdealPowerWatts] decimal(12,2) NOT NULL,
        [IdealPowerKcalPerHour] decimal(12,2) NOT NULL,
        [ConfidenceLevel] int NOT NULL,
        [RequiresProfessionalReview] bit NOT NULL,
        CONSTRAINT [PK_HeatingCalculationResults] PRIMARY KEY ([HeatingCalculationResultId]),
        CONSTRAINT [FK_HeatingCalculationResults_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE TABLE [HeatingInputs] (
        [HeatingInputId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [LengthMeters] decimal(8,2) NOT NULL,
        [WidthMeters] decimal(8,2) NOT NULL,
        [HeightMeters] decimal(8,2) NOT NULL,
        [IsHeightAssumed] bit NOT NULL,
        [ClimateZone] int NOT NULL,
        [InsulationLevel] int NOT NULL,
        [ExteriorWallsCount] int NOT NULL,
        [WindowExposure] int NOT NULL,
        [IsOpenToAnotherSpace] bit NOT NULL,
        CONSTRAINT [PK_HeatingInputs] PRIMARY KEY ([HeatingInputId]),
        CONSTRAINT [FK_HeatingInputs_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE TABLE [HeatingProductAlternatives] (
        [HeatingProductAlternativeId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [SystemType] int NOT NULL,
        [HeatingCapacityWatts] decimal(12,2) NOT NULL,
        [PurchasePrice] decimal(18,2) NOT NULL,
        [EstimatedHourlyCost] decimal(18,2) NOT NULL,
        [EfficiencyLevel] int NOT NULL,
        [SafetyLevel] int NOT NULL,
        CONSTRAINT [PK_HeatingProductAlternatives] PRIMARY KEY ([HeatingProductAlternativeId]),
        CONSTRAINT [FK_HeatingProductAlternatives_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE TABLE [HeatingProductScores] (
        [HeatingProductScoreId] uniqueidentifier NOT NULL,
        [HeatingProductAlternativeId] uniqueidentifier NOT NULL,
        [TotalScore] int NOT NULL,
        [CapacityStatus] int NOT NULL,
        [IsEligible] bit NOT NULL,
        [AppliedMaximumScore] decimal(8,2) NULL,
        CONSTRAINT [PK_HeatingProductScores] PRIMARY KEY ([HeatingProductScoreId]),
        CONSTRAINT [FK_HeatingProductScores_HeatingProductAlternatives_HeatingProductAlternativeId] FOREIGN KEY ([HeatingProductAlternativeId]) REFERENCES [HeatingProductAlternatives] ([HeatingProductAlternativeId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE TABLE [HeatingScoreFactors] (
        [HeatingScoreFactorId] uniqueidentifier NOT NULL,
        [HeatingProductScoreId] uniqueidentifier NOT NULL,
        [FactorType] int NOT NULL,
        [Score] decimal(8,2) NOT NULL,
        [MaximumScore] decimal(8,2) NOT NULL,
        [Explanation] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_HeatingScoreFactors] PRIMARY KEY ([HeatingScoreFactorId]),
        CONSTRAINT [FK_HeatingScoreFactors_HeatingProductScores_HeatingProductScoreId] FOREIGN KEY ([HeatingProductScoreId]) REFERENCES [HeatingProductScores] ([HeatingProductScoreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE UNIQUE INDEX [IX_HeatingCalculationResults_AnalysisId] ON [HeatingCalculationResults] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE UNIQUE INDEX [IX_HeatingInputs_AnalysisId] ON [HeatingInputs] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE INDEX [IX_HeatingProductAlternatives_AnalysisId] ON [HeatingProductAlternatives] ([AnalysisId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE UNIQUE INDEX [IX_HeatingProductScores_HeatingProductAlternativeId] ON [HeatingProductScores] ([HeatingProductAlternativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    CREATE INDEX [IX_HeatingScoreFactors_HeatingProductScoreId] ON [HeatingScoreFactors] ([HeatingProductScoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260804130422_AddHeatingComparison'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260804130422_AddHeatingComparison', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE TABLE [ComparisonAlternatives] (
        [ComparisonAlternativeId] uniqueidentifier NOT NULL,
        [AnalysisId] uniqueidentifier NOT NULL,
        [CategoryCode] int NOT NULL,
        [Position] int NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [TotalCost] decimal(18,2) NULL,
        [DetailsJson] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ComparisonAlternatives] PRIMARY KEY ([ComparisonAlternativeId]),
        CONSTRAINT [FK_ComparisonAlternatives_Analyses_AnalysisId] FOREIGN KEY ([AnalysisId]) REFERENCES [Analyses] ([AnalysisId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE TABLE [ComparisonScores] (
        [ComparisonScoreId] uniqueidentifier NOT NULL,
        [ComparisonAlternativeId] uniqueidentifier NOT NULL,
        [TotalScore] decimal(8,2) NOT NULL,
        [AppliedMaximumScore] decimal(8,2) NULL,
        [IsEligible] bit NOT NULL,
        [StatusCode] nvarchar(100) NULL,
        [DetailsJson] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ComparisonScores] PRIMARY KEY ([ComparisonScoreId]),
        CONSTRAINT [FK_ComparisonScores_ComparisonAlternatives_ComparisonAlternativeId] FOREIGN KEY ([ComparisonAlternativeId]) REFERENCES [ComparisonAlternatives] ([ComparisonAlternativeId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE TABLE [ComparisonFactors] (
        [ComparisonFactorId] uniqueidentifier NOT NULL,
        [ComparisonScoreId] uniqueidentifier NOT NULL,
        [FactorCode] nvarchar(100) NOT NULL,
        [Label] nvarchar(150) NOT NULL,
        [Score] decimal(8,2) NOT NULL,
        [MaximumScore] decimal(8,2) NOT NULL,
        [Weight] decimal(8,4) NULL,
        [Explanation] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_ComparisonFactors] PRIMARY KEY ([ComparisonFactorId]),
        CONSTRAINT [FK_ComparisonFactors_ComparisonScores_ComparisonScoreId] FOREIGN KEY ([ComparisonScoreId]) REFERENCES [ComparisonScores] ([ComparisonScoreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ComparisonAlternatives_AnalysisId_Position] ON [ComparisonAlternatives] ([AnalysisId], [Position]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE INDEX [IX_ComparisonAlternatives_CategoryCode] ON [ComparisonAlternatives] ([CategoryCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ComparisonFactors_ComparisonScoreId_FactorCode] ON [ComparisonFactors] ([ComparisonScoreId], [FactorCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ComparisonScores_ComparisonAlternativeId] ON [ComparisonScores] ([ComparisonAlternativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805015219_AddGenericComparisons'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805015219_AddGenericComparisons', N'10.0.10');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [FlooringScoreFactors];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [HeatingScoreFactors];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [PaintScoreFactors];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [ScoreFactors];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [FlooringProductScores];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [HeatingProductScores];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [PaintProductScores];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [ProductScores];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [FlooringProductAlternatives];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [HeatingProductAlternatives];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [PaintProductAlternatives];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    DROP TABLE [ProductAlternatives];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805023817_RemoveLegacyComparisonTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805023817_RemoveLegacyComparisonTables', N'10.0.10');
END;

COMMIT;
GO
