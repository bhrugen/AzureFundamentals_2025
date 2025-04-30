using System.ComponentModel.DataAnnotations;

namespace AzureBlobProject.Models
{
    public class ContainerModel
    {
        [Required]
        public string Name { get; set; }
    }
}
