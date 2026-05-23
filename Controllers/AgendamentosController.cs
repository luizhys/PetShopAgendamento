using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Data;
using PetShopAgendamento.Filters;
using PetShopAgendamento.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PetShopAgendamento.Models.Agendamento;

namespace PetShopAgendamento.Controllers
{
    [AutorizacaoFilter("Admin", "Funcionario")]
    public class AgendamentosController : Controller
    {
        private readonly AppDbContext _context;

        public AgendamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Agendamentos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Agendamentos.Include(a => a.Cliente).Include(a => a.Pet).Include(a => a.Servico);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Agendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome");
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        // POST: Agendamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,PetId,ServicoId,Data")] Agendamento agendamento, string dataDate, string dataTime)
        {
            // Combina data e hora da view
            if (DateTime.TryParse($"{dataDate} {dataTime}", out var dataCompleta))
            {
                agendamento.Data = dataCompleta;
            }
            else
            {
                ModelState.AddModelError("Data", "Data ou hora inválida.");
                // Recarrega dropdowns e retorna
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
                ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
                ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
                return View(agendamento);
            }

            // Verifica duplicidade de agendamentos
            var existe = await _context.Agendamentos.AnyAsync(a =>
                a.ClienteId == agendamento.ClienteId &&
                a.PetId == agendamento.PetId &&
                a.ServicoId == agendamento.ServicoId &&
                a.Data == agendamento.Data);

            if (existe)
            {
                ModelState.AddModelError(string.Empty, "Já existe um agendamento com os mesmos dados (cliente, pet, serviço e horário).");
                // recarrega dropdowns
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
                ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
                ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
                return View(agendamento);
            }

            // Não permite criar agendamentos no passado
            if (agendamento.Data < DateTime.Now)
            {
                ModelState.AddModelError("Data", "Não é possível agendar serviços em data/hora passada. Escolha uma data futura.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Agendamento feito com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
            return View(agendamento);
        }

        // GET: Agendamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
            return View(agendamento);
        }

        // POST: Agendamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,PetId,ServicoId,Data")] Agendamento agendamento, string dataDate, string dataTime)
        {
            if (id != agendamento.Id)
            {
                return NotFound();
            }

            // Combina data e hora da view
            if (DateTime.TryParse($"{dataDate} {dataTime}", out var dataCompleta))
            {
                agendamento.Data = dataCompleta;
            }
            else
            {
                ModelState.AddModelError("Data", "Data ou hora inválida.");
                // Recarrega dropdowns e retorna
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
                ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
                ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
                return View(agendamento);
            }

            var existe = await _context.Agendamentos.AnyAsync(a =>
                a.Id != agendamento.Id &&
                a.ClienteId == agendamento.ClienteId &&
                a.PetId == agendamento.PetId &&
                a.ServicoId == agendamento.ServicoId &&
                a.Data == agendamento.Data);

            if (existe)
            {
                ModelState.AddModelError(string.Empty, "Já existe um agendamento com os mesmos dados (cliente, pet, serviço e horário).");
                // recarrega dropdowns
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
                ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
                ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
                return View(agendamento);
            }

            // Não permite criar agendamentos no passado
            if (agendamento.Data < DateTime.Now)
            {
                ModelState.AddModelError("Data", "Não é possível agendar serviços em data/hora passada. Escolha uma data futura.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Agendamento atualizado com sucesso.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", agendamento.ClienteId);
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Nome", agendamento.PetId);
            ViewData["ServicoId"] = new SelectList(_context.Servicos, "Id", "Nome", agendamento.ServicoId);
            return View(agendamento);
        }

        // GET: Agendamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Pet)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Agendamento excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.Id == id);
        }
        public async Task<IActionResult> UpdateStatus(int id, StatusAgendamento novoStatus)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
                return NotFound();

            agendamento.Status = novoStatus;
            _context.Update(agendamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
