SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.ComparisonAlternatives', N'U') IS NULL
    THROW 50001, 'Primero ejecute la migracion EF que crea las tablas genericas.', 1;

IF EXISTS (SELECT 1 FROM dbo.ComparisonAlternatives)
    THROW 50002, 'ComparisonAlternatives ya contiene datos. Se cancela para evitar duplicados.', 1;

-- Aire acondicionado
INSERT dbo.ComparisonAlternatives
(
    ComparisonAlternativeId, AnalysisId, CategoryCode, Position, Name,
    TotalCost, DetailsJson
)
SELECT
    p.ProductAlternativeId,
    p.AnalysisId,
    1,
    ROW_NUMBER() OVER (PARTITION BY p.AnalysisId ORDER BY p.ProductAlternativeId),
    p.Name,
    p.Price,
    (SELECT p.Brand AS Brand, p.CapacityFrigories AS CapacityFrigories,
            p.Price AS Price, p.Technology AS Technology,
            p.NominalConsumptionWatts AS NominalConsumptionWatts,
            p.WarrantyMonths AS WarrantyMonths, p.ReferenceUrl AS ReferenceUrl
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.ProductAlternatives p;

INSERT dbo.ComparisonScores
(
    ComparisonScoreId, ComparisonAlternativeId, TotalScore,
    AppliedMaximumScore, IsEligible, StatusCode, DetailsJson
)
SELECT
    s.ProductScoreId, s.ProductAlternativeId, s.TotalScore,
    s.AppliedMaximumScore, s.IsEligible,
    CONVERT(varchar(20), s.CapacityStatus),
    (SELECT s.CapacityStatus AS CapacityStatus,
            s.ConfidenceLevel AS ConfidenceLevel
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.ProductScores s;

INSERT dbo.ComparisonFactors
(
    ComparisonFactorId, ComparisonScoreId, FactorCode, Label,
    Score, MaximumScore, Weight, Explanation
)
SELECT
    f.ScoreFactorId, f.ProductScoreId,
    CONVERT(varchar(30), f.FactorType),
    CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
    f.Score, f.MaximumScore, NULL, f.Explanation
FROM dbo.ScoreFactors f;

-- Pintura
INSERT dbo.ComparisonAlternatives
SELECT
    p.PaintProductAlternativeId, p.AnalysisId, 2,
    ROW_NUMBER() OVER (PARTITION BY p.AnalysisId ORDER BY p.PaintProductAlternativeId),
    p.Name, NULL,
    (SELECT p.ContainerLiters AS ContainerLiters,
            p.PricePerContainer AS PricePerContainer,
            p.CoverageSquareMetersPerLiterPerCoat AS CoverageSquareMetersPerLiterPerCoat,
            p.Washability AS Washability, p.DryingHours AS DryingHours
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.PaintProductAlternatives p;

INSERT dbo.ComparisonScores
SELECT
    s.PaintProductScoreId, s.PaintProductAlternativeId, s.TotalScore,
    NULL, 1, CONVERT(varchar(20), s.CoverageStatus),
    (SELECT s.CoverageStatus AS CoverageStatus,
            s.ConfidenceLevel AS ConfidenceLevel,
            s.ContainersRequired AS ContainersRequired,
            s.LitersRequired AS LitersRequired,
            s.LitersPurchased AS LitersPurchased,
            s.TotalCost AS TotalCost
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.PaintProductScores s;

UPDATE ca
SET TotalCost = TRY_CONVERT(decimal(18,2), JSON_VALUE(cs.DetailsJson, '$.TotalCost'))
FROM dbo.ComparisonAlternatives ca
JOIN dbo.ComparisonScores cs ON cs.ComparisonAlternativeId = ca.ComparisonAlternativeId
WHERE ca.CategoryCode = 2;

INSERT dbo.ComparisonFactors
SELECT
    f.PaintScoreFactorId, f.PaintProductScoreId,
    CONVERT(varchar(30), f.FactorType),
    CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
    f.Score, f.MaximumScore, NULL, f.Explanation
FROM dbo.PaintScoreFactors f;

-- Ceramicos y pisos
INSERT dbo.ComparisonAlternatives
SELECT
    p.FlooringProductAlternativeId, p.AnalysisId, 3,
    ROW_NUMBER() OVER (PARTITION BY p.AnalysisId ORDER BY p.FlooringProductAlternativeId),
    p.Name, NULL,
    (SELECT p.CoverageSquareMetersPerBox AS CoverageSquareMetersPerBox,
            p.PricePerBox AS PricePerBox,
            p.UseResistance AS UseResistance,
            p.ReplacementEase AS ReplacementEase
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.FlooringProductAlternatives p;

INSERT dbo.ComparisonScores
SELECT
    s.FlooringProductScoreId, s.FlooringProductAlternativeId, s.TotalScore,
    NULL, 1, CONVERT(varchar(20), s.CoverageStatus),
    (SELECT s.CoverageStatus AS CoverageStatus,
            s.ConfidenceLevel AS ConfidenceLevel,
            s.BoxesRequired AS BoxesRequired,
            s.RequiredAreaSquareMeters AS RequiredAreaSquareMeters,
            s.PurchasedAreaSquareMeters AS PurchasedAreaSquareMeters,
            s.ExcessAreaSquareMeters AS ExcessAreaSquareMeters,
            s.ExcessPercentage AS ExcessPercentage,
            s.TotalCost AS TotalCost
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.FlooringProductScores s;

UPDATE ca
SET TotalCost = TRY_CONVERT(decimal(18,2), JSON_VALUE(cs.DetailsJson, '$.TotalCost'))
FROM dbo.ComparisonAlternatives ca
JOIN dbo.ComparisonScores cs ON cs.ComparisonAlternativeId = ca.ComparisonAlternativeId
WHERE ca.CategoryCode = 3;

INSERT dbo.ComparisonFactors
SELECT
    f.FlooringScoreFactorId, f.FlooringProductScoreId,
    CONVERT(varchar(30), f.FactorType),
    CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
    f.Score, f.MaximumScore, NULL, f.Explanation
FROM dbo.FlooringScoreFactors f;

-- Calefaccion
INSERT dbo.ComparisonAlternatives
SELECT
    p.HeatingProductAlternativeId, p.AnalysisId, 5,
    ROW_NUMBER() OVER (PARTITION BY p.AnalysisId ORDER BY p.HeatingProductAlternativeId),
    p.Name, p.PurchasePrice,
    (SELECT p.SystemType AS SystemType,
            p.HeatingCapacityWatts AS HeatingCapacityWatts,
            p.PurchasePrice AS PurchasePrice,
            p.EstimatedHourlyCost AS EstimatedHourlyCost,
            p.EfficiencyLevel AS EfficiencyLevel,
            p.SafetyLevel AS SafetyLevel
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.HeatingProductAlternatives p;

INSERT dbo.ComparisonScores
SELECT
    s.HeatingProductScoreId, s.HeatingProductAlternativeId, s.TotalScore,
    s.AppliedMaximumScore, s.IsEligible,
    CONVERT(varchar(20), s.CapacityStatus),
    (SELECT s.CapacityStatus AS CapacityStatus,
            s.IsEligible AS IsEligible
     FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
FROM dbo.HeatingProductScores s;

INSERT dbo.ComparisonFactors
SELECT
    f.HeatingScoreFactorId, f.HeatingProductScoreId,
    CONVERT(varchar(30), f.FactorType),
    CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
    f.Score, f.MaximumScore, NULL, f.Explanation
FROM dbo.HeatingScoreFactors f;

-- Control de integridad
IF (SELECT COUNT(*) FROM dbo.ComparisonAlternatives) <
   ((SELECT COUNT(*) FROM dbo.ProductAlternatives) +
    (SELECT COUNT(*) FROM dbo.PaintProductAlternatives) +
    (SELECT COUNT(*) FROM dbo.FlooringProductAlternatives) +
    (SELECT COUNT(*) FROM dbo.HeatingProductAlternatives))
    THROW 50003, 'La cantidad de alternativas migradas no coincide.', 1;

IF (SELECT COUNT(*) FROM dbo.ComparisonScores) <
   ((SELECT COUNT(*) FROM dbo.ProductScores) +
    (SELECT COUNT(*) FROM dbo.PaintProductScores) +
    (SELECT COUNT(*) FROM dbo.FlooringProductScores) +
    (SELECT COUNT(*) FROM dbo.HeatingProductScores))
    THROW 50004, 'La cantidad de puntajes migrados no coincide.', 1;

COMMIT TRANSACTION;
