namespace Application.Exceptions
{
    public class InvalidImageFormatException(string expectedFormat) : Exception($"Invalid image format. Expected: {expectedFormat}")
    {
        public string ExpectedFormat { get; } = expectedFormat;
    }
}
