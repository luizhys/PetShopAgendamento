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
        [Range(0, 99999.99, ErrorMessage = "Digite um valor numérico válido (use vírgula para decimais).")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }  // precisão configurada no DbContext

        [Required(ErrorMessage = "Duração é obrigatória.")]
        [Display(Name = "Duração")]
        public int Duracao { get; set; }    // minutos

        public ICollection<Agendamento>? Agendamentos { get; set; }
    }
}