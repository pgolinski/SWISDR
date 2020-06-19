namespace SWISDR.Core.CsvConverters
{
    public class IntConverter : ConverterBase<int?>
    {
        public override int? ConvertFromString(string text) => int.Parse(text);
    }
}
