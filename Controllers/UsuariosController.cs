using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Filters;
using PetShopAgendamento.Models;
using PetShopAgendamento.Models.ViewModels;
using PetShopAgendamento.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetShopAgendamento.Controllers
{
    [AutorizacaoFilter("Admin")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cargo,Email,Login,Senha,Perfil")] Usuario usuario)
        {
            // Validação personalizada: senha deve ter pelo menos 4 caracteres
            if (string.IsNullOrEmpty(usuario.Senha) || usuario.Senha.Length < 4)
            {
                ModelState.AddModelError("Senha", "A senha deve ter pelo menos 4 caracteres.");
            }

            if (ModelState.IsValid)
            {
                // Aplica hash na senha
                usuario.Senha = Criptografia.GerarHash(usuario.Senha);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [HttpGet]
        [AutorizacaoFilter("Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var model = new EditUsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Cargo = usuario.Cargo,
                Email = usuario.Email,
                Login = usuario.Login,
                Perfil = usuario.Perfil
            };
            return View(model);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizacaoFilter("Admin")]
        public async Task<IActionResult> Edit(EditUsuarioViewModel model)
        {
            // 1. Carregar o usuário do banco (ANTES de qualquer uso)
            var usuario = await _context.Usuarios.FindAsync(model.Id);
            if (usuario == null) return NotFound();

            // 2. Verificar se o login já existe em outro usuário
            if (_context.Usuarios.Any(u => u.Login == model.Login && u.Id != model.Id))
                ModelState.AddModelError("Login", "Este login já está em uso.");

            // 3. Impedir que o próprio admin mude seu perfil
            var usuarioLogadoId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioLogadoId == model.Id && model.Perfil != usuario.Perfil)
                ModelState.AddModelError("Perfil", "Você não pode alterar seu próprio perfil.");

            // 4. Validação da nova senha (usa o usuario carregado para comparar hash)
            if (!string.IsNullOrEmpty(model.NovaSenha))
            {
                if (model.NovaSenha != model.ConfirmarSenha)
                    ModelState.AddModelError("ConfirmarSenha", "As senhas não conferem.");
                else if (model.NovaSenha.Length < 4)
                    ModelState.AddModelError("NovaSenha", "A senha deve ter pelo menos 4 caracteres.");
                else
                {
                    var novoHash = Criptografia.GerarHash(model.NovaSenha);
                    if (usuario.Senha == novoHash)
                        ModelState.AddModelError("NovaSenha", "A nova senha não pode ser igual à senha atual.");
                    else
                        usuario.Senha = novoHash;
                }
            }

            // 5. Se houver erro de validação, retorna a view com o model
            if (!ModelState.IsValid)
                return View(model);

            // 6. Atualizar os demais campos
            usuario.Nome = model.Nome!;
            usuario.Cargo = model.Cargo!;
            usuario.Email = model.Email;
            usuario.Login = model.Login!;
            usuario.Perfil = model.Perfil;

            // 7. Salvar
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuário atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(u => u.Id == model.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
