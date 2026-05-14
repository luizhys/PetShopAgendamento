using System;
using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione um cliente.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "Selecione um pet.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um pet válido.")]
        [Display(Name = "Pet")]
        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        [Required(ErrorMessage = "Selecione um serviço.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um serviço válido.")]
        [Display(Name = "Serviço")]
        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }

        [Required(ErrorMessage = "Data é obrigatória.")]
        public DateTime Data { get; set; }
    }
}