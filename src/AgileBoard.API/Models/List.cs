using System.ComponentModel.DataAnnotations;

namespace AgileBoard.API.Models
{
    public class List
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        public int BoardId { get; set; }
        public virtual Board Board { get; set; } = null!;

        public int Position { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}