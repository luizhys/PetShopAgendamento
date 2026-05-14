using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Models;
using PetShopAgendamento.Utils;

namespace PetShopAgendamento.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login/Index – exibe o formulário
        [HttpGet]
        public IActionResult Index()
        {
            // Se já estiver logado, redireciona para a Home
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Perfil")))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Login/Index – processa a autenticação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Login e senha são obrigatórios.";
                return View();
            }

            var senhaHash = Criptografia.GerarHash(senha);
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == login && u.Senha == senhaHash);

            if (usuario != null)
            {
                // Armazena dados na sessão
                HttpContext.Session.SetString("Perfil", usuario.Perfil.ToString());
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Login ou senha inválidos.";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}