SET NOCOUNT ON;

DECLARE @Errors TABLE
(
    Orden int IDENTITY(1,1),
    Error nvarchar(4000)
);

IF OBJECT_ID(N'dbo.__EFMigrationsHistory', N'U') IS NULL
    INSERT @Errors(Error) VALUES (N'No existe __EFMigrationsHistory.');

IF OBJECT_ID(N'dbo.ComparisonAlternatives', N'U') IS NULL
    INSERT @Errors(Error) VALUES (N'No existe ComparisonAlternatives.');

IF OBJECT_ID(N'dbo.ComparisonScores', N'U') IS NULL
    INSERT @Errors(Error) VALUES (N'No existe ComparisonScores.');

IF OBJECT_ID(N'dbo.ComparisonFactors', N'U') IS NULL
    INSERT @Errors(Error) VALUES (N'No existe ComparisonFactors.');

IF OBJECT_ID(N'dbo.ProductAlternatives', N'U') IS NOT NULL
    INSERT @Errors(Error) VALUES (N'Todavia existe ProductAlternatives legacy.');

IF OBJECT_ID(N'dbo.PaintProductAlternatives', N'U') IS NOT NULL
    INSERT @Errors(Error) VALUES (N'Todavia existe PaintProductAlternatives legacy.');

IF OBJECT_ID(N'dbo.FlooringProductAlternatives', N'U') IS NOT NULL
    INSERT @Errors(Error) VALUES (N'Todavia existe FlooringProductAlternatives legacy.');

IF OBJECT_ID(N'dbo.HeatingProductAlternatives', N'U') IS NOT NULL
    INSERT @Errors(Error) VALUES (N'Todavia existe HeatingProductAlternatives legacy.');

IF OBJECT_ID(N'dbo.__EFMigrationsHistory', N'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM dbo.__EFMigrationsHistory
       WHERE MigrationId = N'20260805023817_RemoveLegacyComparisonTables'
   )
    INSERT @Errors(Error)
    VALUES (N'EF no registra 20260805023817_RemoveLegacyComparisonTables.');

IF EXISTS (SELECT 1 FROM @Errors)
BEGIN
    SELECT N'ERROR' AS Estado, Orden, Error
    FROM @Errors
    ORDER BY Orden;

    THROW 50100, 'La base no cumple el estado esperado de ElegiBien 1.4.0.', 1;
END;

SELECT
    N'OK - base alineada con ElegiBien 1.4.0' AS Estado,
    (SELECT COUNT(*) FROM dbo.Analyses) AS Analyses,
    (SELECT COUNT(*) FROM dbo.ComparisonAlternatives) AS ComparisonAlternatives,
    (SELECT COUNT(*) FROM dbo.ComparisonScores) AS ComparisonScores,
    (SELECT COUNT(*) FROM dbo.ComparisonFactors) AS ComparisonFactors,
    (SELECT COUNT(*) FROM dbo.SharedResults) AS SharedResults,
    (SELECT COUNT(*) FROM dbo.ConsentRecords) AS ConsentRecords;

SELECT MigrationId, ProductVersion
FROM dbo.__EFMigrationsHistory
ORDER BY MigrationId;
