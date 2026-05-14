using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetShopAgendamento.Filters
{
    public class AutorizacaoFilterAttribute : ActionFilterAttribute
    {
        private readonly string[] _perfisPermitidos;

        public AutorizacaoFilterAttribute(params string[] perfis)
        {
            _perfisPermitidos = perfis;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var perfil = context.HttpContext.Session.GetString("Perfil");
            if (string.IsNullOrEmpty(perfil) || !_perfisPermitidos.Contains(perfil))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}