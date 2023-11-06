using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int weight { get; set; }
        public string code { get; set; }
        [NotMapped]
        [FromForm]
        public IFormFile imagen { get; set; }
    }
}
