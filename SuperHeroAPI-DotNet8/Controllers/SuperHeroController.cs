using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI_DotNet8.Data;
using SuperHeroAPI_DotNet8.Entities;
using SuperHeroAPI_DotNet8.Interfaces;
using SuperHeroAPI_DotNet8.Services;

namespace SuperHeroAPI_DotNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogWriter _logWriter;

        public SuperHeroController(DataContext context, ILogWriter logWriter)
        {
            _context = context;
            _logWriter = logWriter;
        }

        private void LogHeroProperties(SuperHero hero)
        {
            var properties = hero.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(hero, null) ?? "null";
                _logWriter.WriteLog($"{propName}: {propValue}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            _logWriter.WriteLog("GET ALL");
            var heroes = await _context.SuperHeroes.ToListAsync();
            _logWriter.WriteLog($"Na lista tem {heroes.Count} heróis.\n");
            foreach (var hero in heroes)
            {
                LogHeroProperties(hero);

            }
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            _logWriter.WriteLog($"GET ID do herói com ID {id}.");
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero is null)
            {
                _logWriter.WriteLog($"Herói com ID {id} não encontrado.");
                return NotFound("Hero not found.\n");
            }
            _logWriter.WriteLog($"Herói com ID {id} encontrado: {hero.Name}.\n");
            LogHeroProperties(hero);
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _logWriter.WriteLog($"(POST)Adicionando novo herói: {hero.Name}.");
            LogHeroProperties(hero);
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updatedHero)
        {
            _logWriter.WriteLog($"(PUT)Atualizando o herói com ID {updatedHero.Id}.\n");
            var dbHero = await _context.SuperHeroes.FindAsync(updatedHero.Id);
            if (dbHero is null)
            {
                _logWriter.WriteLog($"Herói com ID {updatedHero.Id} não encontrado.");
                return NotFound("Hero not found.\n");
            }
            dbHero.Name = updatedHero.Name;
            dbHero.FirstName = updatedHero.FirstName;
            dbHero.LastName = updatedHero.LastName;
            dbHero.Place = updatedHero.Place;

            _logWriter.WriteLog("Propriedades do herói atualizadas:");
            LogHeroProperties(dbHero);
            await _context.SaveChangesAsync();
            _logWriter.WriteLog($"Herói {dbHero.Name} atualizado com sucesso.\n");

            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            _logWriter.WriteLog($"(Tentativa de exclusão do herói com ID {id}.");
            var dbHero = await _context.SuperHeroes.FindAsync(id);
            if (dbHero is null)
            {
                _logWriter.WriteLog($"Herói com ID {id} não encontrado.");
                return NotFound("Hero not found.\n");
            }
            _logWriter.WriteLog($"Herói encontrado para exclusão: {dbHero.Name}.\n");
            LogHeroProperties(dbHero);

            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();
            _logWriter.WriteLog($"Herói {dbHero.Name} excluído com sucesso.\n");

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
