namespace Shoppingendly.Services.Products.Application.DTO
{
    public class PictureDto
    {
        public string Url { get; }

        public PictureDto(string url)
        {
            Url = url;
        }
    }
}