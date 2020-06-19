namespace SWISDR.Core.CsvConverters
{
    public class StringConverter : ConverterBase<string>
    {
        public override string ConvertFromString(string text) => text.Replace("_", " ");

        public override string ConvertToString(string value) => value.Replace(" ", "_");
    }
}
