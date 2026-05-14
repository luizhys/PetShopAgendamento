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

        public enum PorteOpcao
        {
            Pequeno,

            [Display(Name = "Médio")]
            Medio,

            Grande
        }

        [Required(ErrorMessage = "Porte é obrigatório.")]
        public PorteOpcao Porte { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        public decimal? Peso { get; set; }   // precisão configurada no DbContext

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