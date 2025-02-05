using System.ComponentModel.DataAnnotations;

namespace AgileBoard.API.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}