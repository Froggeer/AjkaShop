namespace Ajka.Common.Helpers
{
    public static class ConvertConstantToDescriptionHelper
    {
        public static string ConvertSexTypeToDescription(string sexType)
        {
            return sexType switch
            {
                "LADIES" => " Dámské",
                "GENTS" => " Pánské",
                "UNISEX" => " Unisex",
                "KIDS" => " Dětské",
                "DOPLNKY" => " Doplňky",
                "OBAL" => " Obal",
                _ => sexType,
            };
        }

        public static string ConvertProductLabelToDescription(string productLabel)
        {
            return productLabel switch
            {
                "MALFINI" => "Malfini",
                "MALFINIPREMIUM" => "Malfini premium",
                "RIMECK" => "Rimeck",
                "PICCOLIO" => "Piccolio",
                _ => productLabel,
            };
        }
    }
}
