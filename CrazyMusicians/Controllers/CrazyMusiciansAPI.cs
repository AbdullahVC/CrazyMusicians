using CrazyMusicians.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CrazyMusiciansAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        // In-memory data simulation
        private static List<Musician> Musicians = new List<Musician>
        {
            new Musician { Id = 1, Name = "Ahmet Çalgı", Profession = "Ünlü Çalgı Çalar", FunFact = "Her zaman yanlış nota çalar, ama çok eğlenceli" },
            new Musician { Id = 2, Name = "Zeynep Melodi", Profession = "Popüler Melodi Yazarı", FunFact = "Şarkıları yanlış anlaşılır ama çok popüler" },
            new Musician { Id = 3, Name = "Cemil Akor", Profession = "Çılgın Akorist", FunFact = "Akorları sık değiştirir, ama şaşırtıcı derecede yetenekli" },
            new Musician { Id = 4, Name = "Fatma Nota", Profession = "Sürpriz Nota Üreticisi", FunFact = "Nota üretirken sürekli sürprizler hazırlar" },
            new Musician { Id = 5, Name = "Hasan Ritim", Profession = "Ritim Canavarı", FunFact = "Her ritmi kendi tarzında yapar, hiç uymaz ama komiktir" },
            new Musician { Id = 6, Name = "Elif Armoni", Profession = "Armoni Ustası", FunFact = "Armonilerini bazen yanlış çalar, ama çok yaratıcıdır" },
            new Musician { Id = 7, Name = "Ali Perde", Profession = "Perde Uygulayıcı", FunFact = "Her perdeyi farklı şekilde çalar; her zaman sürprizlidir" },
            new Musician { Id = 8, Name = "Ayşe Rezonans", Profession = "Rezonans Uzmanı", FunFact = "Rezonans konusunda uzman, ama bazen çok gürültü çıkarır" },
            new Musician { Id = 9, Name = "Murat Ton", Profession = "Tonlama Meraklısı", FunFact = "Tonlamalarındaki farklılıklar bazen komik, ama oldukça ilginç" },
            new Musician { Id = 10, Name = "Selin Akor", Profession = "Akor Sihirbazı", FunFact = "Akorları değiştirdiğinde bazen sihirli bir hava yaratır" }
        };

        // Get all musicians
        [HttpGet]
        public ActionResult<IEnumerable<Musician>> GetAllMusicians()
        {
            return Ok(Musicians);
        }

        // Get a musician by ID
        [HttpGet("{id}")]
        public ActionResult<Musician> GetMusicianById(int id)
        {
            var musician = Musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with ID {id} not found.");
            }

            return Ok(musician);
        }

        // Add a new musician
        [HttpPost]
        public ActionResult<Musician> CreateMusician([FromBody] Musician newMusician)
        {
            newMusician.Id = Musicians.Max(m => m.Id) + 1; // Assign a new ID
            Musicians.Add(newMusician);

            // Return the URI for the newly created musician
            return CreatedAtAction(nameof(GetMusicianById), new { id = newMusician.Id }, newMusician);
        }

        // Update an existing musician
        [HttpPut("{id}")]
        public IActionResult UpdateMusician(int id, [FromBody] Musician updatedMusician)
        {
            var musician = Musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with ID {id} not found.");
            }

            // Update musician
            musician.Name = updatedMusician.Name;
            musician.Profession = updatedMusician.Profession;
            musician.FunFact = updatedMusician.FunFact;

            return NoContent(); // Successful update
        }

        // Update only the fun fact (Patch)
        [HttpPatch("{id}")]
        public IActionResult UpdateFunFact(int id, [FromBody] string newFunFact)
        {
            var musician = Musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with ID {id} not found.");
            }

            musician.FunFact = newFunFact; // Update fun fact
            return NoContent();
        }

        // Delete a musician
        [HttpDelete("{id}")]
        public IActionResult DeleteMusician(int id)
        {
            var musician = Musicians.FirstOrDefault(m => m.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician with ID {id} not found.");
            }

            Musicians.Remove(musician); // Remove from the list
            return NoContent();
        }

        // Search musicians by profession
        [HttpGet("search")]
        public ActionResult<IEnumerable<Musician>> SearchMusicians([FromQuery] string profession)
        {
            var matchingMusicians = Musicians.Where(m => m.Profession.ToLower().Contains(profession.ToLower())).ToList();
            if (!matchingMusicians.Any())
            {
                return NotFound($"No musicians found with the profession '{profession}'.");
            }

            return Ok(matchingMusicians);
        }
    }
}
