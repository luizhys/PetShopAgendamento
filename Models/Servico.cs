using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models
{
    public class Servico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório.")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }  // precisão configurada no DbContext

        [Required(ErrorMessage = "Duração é obrigatória.")]
        [Display(Name = "Duração")]
        public int Duracao { get; set; }    // minutos

        public ICollection<Agendamento>? Agendamentos { get; set; }
    }
}