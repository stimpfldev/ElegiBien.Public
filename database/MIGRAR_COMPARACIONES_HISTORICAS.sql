SET NOCOUNT ON;
SET XACT_ABORT ON;

/*
    ElegiBien - migracion de comparaciones historicas

    Objetivo:
    - Copiar datos de las tablas legacy a las tablas genericas.
    - Poder ejecutarse nuevamente sin duplicar datos.
    - No borrar tablas legacy: esa responsabilidad sigue siendo de EF Core.
    - Detectar un esquema parcial/inconsistente antes de modificar datos.

    Secuencia de upgrade esperada para una base antigua:
    1) Aplicar EF hasta 20260805015219_AddGenericComparisons.
    2) Ejecutar este script.
    3) Validar el resultado.
    4) Aplicar las migraciones EF restantes.
*/

IF OBJECT_ID(N'dbo.ComparisonAlternatives', N'U') IS NULL
   OR OBJECT_ID(N'dbo.ComparisonScores', N'U') IS NULL
   OR OBJECT_ID(N'dbo.ComparisonFactors', N'U') IS NULL
BEGIN
    THROW 50001, 'Primero ejecute la migracion EF 20260805015219_AddGenericComparisons.', 1;
END;

DECLARE @LegacyTableCount int =
    (CASE WHEN OBJECT_ID(N'dbo.ProductAlternatives', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.ProductScores', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.ScoreFactors', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.PaintProductAlternatives', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.PaintProductScores', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.PaintScoreFactors', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.FlooringProductAlternatives', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.FlooringProductScores', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.FlooringScoreFactors', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.HeatingProductAlternatives', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.HeatingProductScores', N'U') IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN OBJECT_ID(N'dbo.HeatingScoreFactors', N'U') IS NOT NULL THEN 1 ELSE 0 END);

-- Si EF ya elimino las 12 tablas legacy, no hay nada que copiar.
IF @LegacyTableCount = 0
BEGIN
    SELECT
        N'OK - esquema legacy ya eliminado' AS Estado,
        (SELECT COUNT(*) FROM dbo.ComparisonAlternatives) AS ComparisonAlternatives,
        (SELECT COUNT(*) FROM dbo.ComparisonScores) AS ComparisonScores,
        (SELECT COUNT(*) FROM dbo.ComparisonFactors) AS ComparisonFactors;
    RETURN;
END;

-- Un estado parcial implica que no es seguro continuar automaticamente.
IF @LegacyTableCount <> 12
BEGIN
    THROW 50002, 'El esquema legacy esta incompleto. Se cancela la migracion para evitar perdida de datos.', 1;
END;

BEGIN TRY
    BEGIN TRANSACTION;

    /* Aire acondicionado */
    ;WITH SourceRows AS
    (
        SELECT
            p.ProductAlternativeId,
            p.AnalysisId,
            ROW_NUMBER() OVER
                (PARTITION BY p.AnalysisId ORDER BY p.ProductAlternativeId) AS Position,
            p.Name,
            p.Price,
            (SELECT p.Brand AS Brand,
                    p.CapacityFrigories AS CapacityFrigories,
                    p.Price AS Price,
                    p.Technology AS Technology,
                    p.NominalConsumptionWatts AS NominalConsumptionWatts,
                    p.WarrantyMonths AS WarrantyMonths,
                    p.ReferenceUrl AS ReferenceUrl
             FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DetailsJson
        FROM dbo.ProductAlternatives p
    )
    INSERT dbo.ComparisonAlternatives
    (
        ComparisonAlternativeId, AnalysisId, CategoryCode, Position, Name,
        TotalCost, DetailsJson
    )
    SELECT
        s.ProductAlternativeId, s.AnalysisId, 1, s.Position, s.Name,
        s.Price, s.DetailsJson
    FROM SourceRows s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonAlternatives ca
        WHERE ca.ComparisonAlternativeId = s.ProductAlternativeId
    );

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
    FROM dbo.ProductScores s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonScores cs
        WHERE cs.ComparisonScoreId = s.ProductScoreId
    );

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
    FROM dbo.ScoreFactors f
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonFactors cf
        WHERE cf.ComparisonFactorId = f.ScoreFactorId
    );

    /* Pintura */
    ;WITH SourceRows AS
    (
        SELECT
            p.PaintProductAlternativeId,
            p.AnalysisId,
            ROW_NUMBER() OVER
                (PARTITION BY p.AnalysisId ORDER BY p.PaintProductAlternativeId) AS Position,
            p.Name,
            (SELECT p.ContainerLiters AS ContainerLiters,
                    p.PricePerContainer AS PricePerContainer,
                    p.CoverageSquareMetersPerLiterPerCoat AS CoverageSquareMetersPerLiterPerCoat,
                    p.Washability AS Washability,
                    p.DryingHours AS DryingHours
             FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DetailsJson
        FROM dbo.PaintProductAlternatives p
    )
    INSERT dbo.ComparisonAlternatives
    (
        ComparisonAlternativeId, AnalysisId, CategoryCode, Position, Name,
        TotalCost, DetailsJson
    )
    SELECT
        s.PaintProductAlternativeId, s.AnalysisId, 2, s.Position, s.Name,
        NULL, s.DetailsJson
    FROM SourceRows s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonAlternatives ca
        WHERE ca.ComparisonAlternativeId = s.PaintProductAlternativeId
    );

    INSERT dbo.ComparisonScores
    (
        ComparisonScoreId, ComparisonAlternativeId, TotalScore,
        AppliedMaximumScore, IsEligible, StatusCode, DetailsJson
    )
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
    FROM dbo.PaintProductScores s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonScores cs
        WHERE cs.ComparisonScoreId = s.PaintProductScoreId
    );

    UPDATE ca
    SET TotalCost = TRY_CONVERT(decimal(18,2), JSON_VALUE(cs.DetailsJson, '$.TotalCost'))
    FROM dbo.ComparisonAlternatives ca
    JOIN dbo.ComparisonScores cs
        ON cs.ComparisonAlternativeId = ca.ComparisonAlternativeId
    WHERE ca.CategoryCode = 2;

    INSERT dbo.ComparisonFactors
    (
        ComparisonFactorId, ComparisonScoreId, FactorCode, Label,
        Score, MaximumScore, Weight, Explanation
    )
    SELECT
        f.PaintScoreFactorId, f.PaintProductScoreId,
        CONVERT(varchar(30), f.FactorType),
        CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
        f.Score, f.MaximumScore, NULL, f.Explanation
    FROM dbo.PaintScoreFactors f
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonFactors cf
        WHERE cf.ComparisonFactorId = f.PaintScoreFactorId
    );

    /* Ceramicos y pisos */
    ;WITH SourceRows AS
    (
        SELECT
            p.FlooringProductAlternativeId,
            p.AnalysisId,
            ROW_NUMBER() OVER
                (PARTITION BY p.AnalysisId ORDER BY p.FlooringProductAlternativeId) AS Position,
            p.Name,
            (SELECT p.CoverageSquareMetersPerBox AS CoverageSquareMetersPerBox,
                    p.PricePerBox AS PricePerBox,
                    p.UseResistance AS UseResistance,
                    p.ReplacementEase AS ReplacementEase
             FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DetailsJson
        FROM dbo.FlooringProductAlternatives p
    )
    INSERT dbo.ComparisonAlternatives
    (
        ComparisonAlternativeId, AnalysisId, CategoryCode, Position, Name,
        TotalCost, DetailsJson
    )
    SELECT
        s.FlooringProductAlternativeId, s.AnalysisId, 3, s.Position, s.Name,
        NULL, s.DetailsJson
    FROM SourceRows s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonAlternatives ca
        WHERE ca.ComparisonAlternativeId = s.FlooringProductAlternativeId
    );

    INSERT dbo.ComparisonScores
    (
        ComparisonScoreId, ComparisonAlternativeId, TotalScore,
        AppliedMaximumScore, IsEligible, StatusCode, DetailsJson
    )
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
    FROM dbo.FlooringProductScores s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonScores cs
        WHERE cs.ComparisonScoreId = s.FlooringProductScoreId
    );

    UPDATE ca
    SET TotalCost = TRY_CONVERT(decimal(18,2), JSON_VALUE(cs.DetailsJson, '$.TotalCost'))
    FROM dbo.ComparisonAlternatives ca
    JOIN dbo.ComparisonScores cs
        ON cs.ComparisonAlternativeId = ca.ComparisonAlternativeId
    WHERE ca.CategoryCode = 3;

    INSERT dbo.ComparisonFactors
    (
        ComparisonFactorId, ComparisonScoreId, FactorCode, Label,
        Score, MaximumScore, Weight, Explanation
    )
    SELECT
        f.FlooringScoreFactorId, f.FlooringProductScoreId,
        CONVERT(varchar(30), f.FactorType),
        CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
        f.Score, f.MaximumScore, NULL, f.Explanation
    FROM dbo.FlooringScoreFactors f
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonFactors cf
        WHERE cf.ComparisonFactorId = f.FlooringScoreFactorId
    );

    /* Calefaccion */
    ;WITH SourceRows AS
    (
        SELECT
            p.HeatingProductAlternativeId,
            p.AnalysisId,
            ROW_NUMBER() OVER
                (PARTITION BY p.AnalysisId ORDER BY p.HeatingProductAlternativeId) AS Position,
            p.Name,
            p.PurchasePrice,
            (SELECT p.SystemType AS SystemType,
                    p.HeatingCapacityWatts AS HeatingCapacityWatts,
                    p.PurchasePrice AS PurchasePrice,
                    p.EstimatedHourlyCost AS EstimatedHourlyCost,
                    p.EfficiencyLevel AS EfficiencyLevel,
                    p.SafetyLevel AS SafetyLevel
             FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DetailsJson
        FROM dbo.HeatingProductAlternatives p
    )
    INSERT dbo.ComparisonAlternatives
    (
        ComparisonAlternativeId, AnalysisId, CategoryCode, Position, Name,
        TotalCost, DetailsJson
    )
    SELECT
        s.HeatingProductAlternativeId, s.AnalysisId, 5, s.Position, s.Name,
        s.PurchasePrice, s.DetailsJson
    FROM SourceRows s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonAlternatives ca
        WHERE ca.ComparisonAlternativeId = s.HeatingProductAlternativeId
    );

    INSERT dbo.ComparisonScores
    (
        ComparisonScoreId, ComparisonAlternativeId, TotalScore,
        AppliedMaximumScore, IsEligible, StatusCode, DetailsJson
    )
    SELECT
        s.HeatingProductScoreId, s.HeatingProductAlternativeId, s.TotalScore,
        s.AppliedMaximumScore, s.IsEligible,
        CONVERT(varchar(20), s.CapacityStatus),
        (SELECT s.CapacityStatus AS CapacityStatus,
                s.IsEligible AS IsEligible
         FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM dbo.HeatingProductScores s
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonScores cs
        WHERE cs.ComparisonScoreId = s.HeatingProductScoreId
    );

    INSERT dbo.ComparisonFactors
    (
        ComparisonFactorId, ComparisonScoreId, FactorCode, Label,
        Score, MaximumScore, Weight, Explanation
    )
    SELECT
        f.HeatingScoreFactorId, f.HeatingProductScoreId,
        CONVERT(varchar(30), f.FactorType),
        CONCAT('Factor ', CONVERT(varchar(30), f.FactorType)),
        f.Score, f.MaximumScore, NULL, f.Explanation
    FROM dbo.HeatingScoreFactors f
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.ComparisonFactors cf
        WHERE cf.ComparisonFactorId = f.HeatingScoreFactorId
    );

    /* Controles de integridad por ID, no solo por cantidad. */
    IF EXISTS
    (
        SELECT 1 FROM dbo.ProductAlternatives p
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonAlternatives ca
            WHERE ca.ComparisonAlternativeId = p.ProductAlternativeId
              AND ca.CategoryCode = 1
        )
    )
        THROW 50003, 'Faltan alternativas historicas de aire acondicionado.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.PaintProductAlternatives p
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonAlternatives ca
            WHERE ca.ComparisonAlternativeId = p.PaintProductAlternativeId
              AND ca.CategoryCode = 2
        )
    )
        THROW 50004, 'Faltan alternativas historicas de pintura.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.FlooringProductAlternatives p
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonAlternatives ca
            WHERE ca.ComparisonAlternativeId = p.FlooringProductAlternativeId
              AND ca.CategoryCode = 3
        )
    )
        THROW 50005, 'Faltan alternativas historicas de ceramicos/pisos.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.HeatingProductAlternatives p
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonAlternatives ca
            WHERE ca.ComparisonAlternativeId = p.HeatingProductAlternativeId
              AND ca.CategoryCode = 5
        )
    )
        THROW 50006, 'Faltan alternativas historicas de calefaccion.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.ProductScores s
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonScores cs
            WHERE cs.ComparisonScoreId = s.ProductScoreId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.PaintProductScores s
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonScores cs
            WHERE cs.ComparisonScoreId = s.PaintProductScoreId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.FlooringProductScores s
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonScores cs
            WHERE cs.ComparisonScoreId = s.FlooringProductScoreId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.HeatingProductScores s
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonScores cs
            WHERE cs.ComparisonScoreId = s.HeatingProductScoreId
        )
    )
        THROW 50007, 'Faltan puntajes historicos en las tablas genericas.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.ScoreFactors f
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonFactors cf
            WHERE cf.ComparisonFactorId = f.ScoreFactorId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.PaintScoreFactors f
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonFactors cf
            WHERE cf.ComparisonFactorId = f.PaintScoreFactorId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.FlooringScoreFactors f
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonFactors cf
            WHERE cf.ComparisonFactorId = f.FlooringScoreFactorId
        )
    ) OR EXISTS
    (
        SELECT 1 FROM dbo.HeatingScoreFactors f
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.ComparisonFactors cf
            WHERE cf.ComparisonFactorId = f.HeatingScoreFactorId
        )
    )
        THROW 50008, 'Faltan factores historicos en las tablas genericas.', 1;

    COMMIT TRANSACTION;

    SELECT
        N'OK - comparaciones historicas migradas y verificadas' AS Estado,
        (SELECT COUNT(*) FROM dbo.ComparisonAlternatives) AS ComparisonAlternatives,
        (SELECT COUNT(*) FROM dbo.ComparisonScores) AS ComparisonScores,
        (SELECT COUNT(*) FROM dbo.ComparisonFactors) AS ComparisonFactors;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
