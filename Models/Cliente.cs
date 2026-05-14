using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        public required string Telefone { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        [Display(Name = "Endereço")]
        public required string Endereco { get; set; }

        // Navegações
        public ICollection<Pet>? Pets { get; set; }
        public ICollection<Agendamento>? Agendamentos { get; set; }
    }
}