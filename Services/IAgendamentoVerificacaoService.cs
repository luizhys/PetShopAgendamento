using PetShopAgendamento.Models;

namespace PetShopAgendamento.Services
{
    public interface IAgendamentoVerificacaoService
    {
        Task<bool> ClientePossuiAgendamentosOuPetsAsync(int clienteId);
        Task<bool> PetPossuiAgendamentosAsync(int petId);
        Task<bool> ServicoPossuiAgendamentosAsync(int servicoId);
    }
}