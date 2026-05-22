namespace PetShopAgendamento.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; } = 0;
        public int TotalPets { get; set; } = 0;
        public int AgendamentosHoje { get; set; } = 0;
        public int ServicosAtivos { get; set; } = 0;
        public List<ProximoAgendamento> ProximosAgendamentos { get; set; } = new();
    }

    public class ProximoAgendamento
    {
        public DateTime Data { get; set; } = DateTime.Now;
        public string ClienteNome { get; set; } = string.Empty;
        public string PetNome { get; set; } = string.Empty;
        public string ServicoNome { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}