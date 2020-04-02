using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.EnumEntities
{
    public class AnalysisFlag : AbstractEnumEntity
    {
        
    }

    /*
    public void ConfigureDefaultValues(DefaultValueBuilder builder)
    {
        var values = new []
        {
            new AnalysisFlag { Key = "insufficent-vegetables", Name = "Ungenügende Gemüseaufnahme" },
            new AnalysisFlag { Key = "high-sugar", Name = "Erhöhte Zuckeraufnahme" },
            new AnalysisFlag { Key = "unbalanced-protein", Name = "Einseitige Proteinaufnahme" },
            new AnalysisFlag { Key = "missing-meals", Name = "Ausgelassene Mahlzeiten" },
            new AnalysisFlag { Key = "too-many-meals", Name = "Zuviele Mahlzeiten" },
        };

        builder.AddMany(values);
    }
    */
}
