using System.ComponentModel.DataAnnotations;

namespace AgileBoard.API.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        public string? Description { get; set; }

        public int ListId { get; set; }
        public virtual List List { get; set; } = null!;

        public int? AssignedUserId { get; set; }
        public virtual User? AssignedUser { get; set; }

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}