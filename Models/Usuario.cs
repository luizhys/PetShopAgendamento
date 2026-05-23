using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models
{
    public enum Perfil
    {
        [Display(Name = "Administrador")]
        Admin,
        [Display(Name = "Funcionário")]
        Funcionario
    }

    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Cargo é obrigatório.")]
        public required string Cargo { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Login é obrigatório.")]
        public required string Login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public required string Senha { get; set; } // será armazenada com hash

        [Required(ErrorMessage = "Selecione um perfil.")]
        [Range(0, int.MaxValue, ErrorMessage = "Selecione um perfil válido.")]
        public Perfil Perfil { get; set; }
    }
}