using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class GachaCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Number { get; set; }
        public DateTime Created { get; set; }
    }
}
