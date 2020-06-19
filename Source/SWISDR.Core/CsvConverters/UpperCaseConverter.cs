namespace SWISDR.Core.CsvConverters
{
    public class UpperCaseConverter : ConverterBase<string>
    {
        public override string ConvertFromString(string text) => text.ToUpper();
    }
}
