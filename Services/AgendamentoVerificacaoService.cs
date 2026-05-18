using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Models;

namespace PetShopAgendamento.Services
{
    public class AgendamentoVerificacaoService : IAgendamentoVerificacaoService
    {
        private readonly AppDbContext _context;

        public AgendamentoVerificacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ClientePossuiAgendamentosOuPetsAsync(int clienteId)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Agendamentos)
                .Include(c => c.Pets)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null) return false;

            // Retorna true se houver agendamentos OU se houver pets (qualquer quantidade)
            return (cliente.Agendamentos != null && cliente.Agendamentos.Any()) ||
                   (cliente.Pets != null && cliente.Pets.Any());
        }

        public async Task<bool> PetPossuiAgendamentosAsync(int petId)
        {
            var pet = await _context.Pets
                .Include(p => p.Agendamentos)
                .FirstOrDefaultAsync(p => p.Id == petId);

            return pet != null && pet.Agendamentos != null && pet.Agendamentos.Any();
        }

        public async Task<bool> ServicoPossuiAgendamentosAsync(int servicoId)
        {
            var servico = await _context.Servicos
                .Include(s => s.Agendamentos)
                .FirstOrDefaultAsync(s => s.Id == servicoId);

            return servico != null && servico.Agendamentos != null && servico.Agendamentos.Any();
        }
    }
}