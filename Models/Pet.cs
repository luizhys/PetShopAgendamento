using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models
{
    public class Pet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Display(Name = "Raça")]
        public string? Raca { get; set; }

        [Required(ErrorMessage = "Porte é obrigatório.")]
        public required string Porte { get; set; }  // Pequeno, Médio, Grande

        public decimal Peso { get; set; }   // precisão configurada no DbContext

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "Selecione um cliente.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public ICollection<Agendamento>? Agendamentos { get; set; }
    }
}