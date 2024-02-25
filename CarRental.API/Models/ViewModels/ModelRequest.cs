using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Models.ViewModels
{
    public class ModelRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MakeId { get; set; }
    }
}
