using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Filters;
using PetShopAgendamento.Models.ViewModels; // ViewModel que será criado

namespace PetShopAgendamento.Controllers
{
    [AutorizacaoFilter("Admin", "Funcionario")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Today;
            var viewModel = new DashboardViewModel
            {
                TotalClientes = await _context.Clientes.CountAsync(),
                TotalPets = await _context.Pets.CountAsync(),
                AgendamentosHoje = await _context.Agendamentos.CountAsync(a => a.Data.Date == hoje),
                ServicosAtivos = await _context.Servicos.CountAsync(),
                ProximosAgendamentos = await _context.Agendamentos
                    .Where(a => a.Data >= DateTime.Now)
                    .OrderBy(a => a.Data)
                    .Take(5)
                    .Select(a => new ProximoAgendamento
                    {
                        Data = a.Data,
                        ClienteNome = a.Cliente != null ? a.Cliente.Nome : "N/A",
                        PetNome = a.Pet != null ? a.Pet.Nome : "N/A",
                        ServicoNome = a.Servico != null ? a.Servico.Nome : "N/A",
                        Status = a.Status.ToString()
                    })
                    .ToListAsync()
            };
            return View(viewModel);
        }
    }
}