using System.ComponentModel.DataAnnotations;

namespace PetShopAgendamento.Models.ViewModels
{
    public class EditUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string? Nome { get; set; }

        public string? Cargo { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Login é obrigatório")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Perfil é obrigatório")]
        public Perfil Perfil { get; set; }

        // Campos de senha (opcionais)
        public string? NovaSenha { get; set; }

        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem.")]
        public string? ConfirmarSenha { get; set; }
    }
}