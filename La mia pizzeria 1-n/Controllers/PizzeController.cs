using La_mia_pizzeria_1_n.Database;
using La_mia_pizzeria_1_n.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace La_mia_pizzeria_1_n.Controllers {
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            using (PizzaContext db = new PizzaContext()) {
                List<Pizza> Pizze = db.Pizze.Include(articolo => articolo.Tags).ToList<Pizza>();

                return Ok(Pizze);
            }
        }


    }
}
