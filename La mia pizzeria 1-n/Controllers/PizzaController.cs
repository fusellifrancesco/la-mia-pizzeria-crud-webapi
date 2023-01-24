using La_mia_pizzeria_1_n.Database;
using Microsoft.AspNetCore.Mvc;
using La_mia_pizzeria_1_n.Models;
using Microsoft.EntityFrameworkCore;
using La_mia_pizzeria_1_n.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace La_mia_pizzeria_1_n.Controllers {

    [Authorize]
    public class PizzaController : Controller {

        public IActionResult Index() {

            using (PizzaContext db = new PizzaContext()) {

                List<Pizza> ListaPizze = db.Pizze.ToList<Pizza>();

                return View("Index", ListaPizze);
            }

        }

        public IActionResult Details(int id) {

            using (PizzaContext db = new PizzaContext()) {

                Pizza PizzaTrovata = db.Pizze
                    .Where(SingolaPizzaNelDb => SingolaPizzaNelDb.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Tags)
                    .FirstOrDefault();

                if (PizzaTrovata != null) {

                    return View(PizzaTrovata);
                }

                return NotFound("La pizza con l'id cercato non esiste");
            }
        }

        [HttpGet]
        public IActionResult Create() {

            using (PizzaContext db = new PizzaContext()) {
                
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = new Pizza();

                modelForView.Categories = categoriesFromDb;
                modelForView.Tags = TagsConverter.getListTagsForMultipleSelect();

                return View("Create", modelForView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategoriesView formData) {
            if (!ModelState.IsValid) {
                using (PizzaContext db = new PizzaContext()) {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;

                    formData.Tags = TagsConverter.getListTagsForMultipleSelect();
                }


                return View("Create", formData);
            }

            using (PizzaContext db = new PizzaContext()) {
                if (formData.TagsSelectedFromMultipleSelect != null) {
                    formData.Pizza.Tags = new List<Tag>();

                    foreach (string tagId in formData.TagsSelectedFromMultipleSelect) {
                        int tagIdIntFromSelect = int.Parse(tagId);

                        Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                        // todo controllare eventuali altri errori tipo l'id del tag non esiste

                        formData.Pizza.Tags.Add(tag);
                    }
                }

                db.Pizze.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    

        [HttpGet]
        public IActionResult Update(int id) {
            using (PizzaContext db = new PizzaContext()) {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Tags).FirstOrDefault();

                if (pizzaToUpdate == null) {
                    return NotFound("La pizza non è stata trovata");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = pizzaToUpdate;
                modelForView.Categories = categories;


                List<Tag> listTagFromDb = db.Tags.ToList<Tag>();

                List<SelectListItem> listaOpzioniPerLaSelect = new List<SelectListItem>();

                foreach (Tag tag in listTagFromDb) {
                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare
                    bool eraStatoSelezionato = pizzaToUpdate.Tags.Any(tagSelezionati => tagSelezionati.Id == tag.Id);

                    SelectListItem opzioneSingolaSelect = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString(), Selected = eraStatoSelezionato };
                    listaOpzioniPerLaSelect.Add(opzioneSingolaSelect);
                }

                modelForView.Tags = listaOpzioniPerLaSelect;

                return View("Update", modelForView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaCategoriesView formData) {
            if (!ModelState.IsValid) {

                using (PizzaContext db = new PizzaContext()) {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }

                return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext()) {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Tags).FirstOrDefault();

                if (pizzaToUpdate != null) {

                    pizzaToUpdate.Name = formData.Pizza.Name;
                    pizzaToUpdate.Description = formData.Pizza.Description;
                    pizzaToUpdate.Img = formData.Pizza.Img;
                    pizzaToUpdate.CategoryId = formData.Pizza.CategoryId;

                    // rimuoviamo i tag e inseriamo i nuovi
                    pizzaToUpdate.Tags.Clear();

                    if (formData.TagsSelectedFromMultipleSelect != null) {

                        foreach (string tagId in formData.TagsSelectedFromMultipleSelect) {
                            int tagIdIntFromSelect = int.Parse(tagId);

                            Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                            // todo controllare eventuali altri errori tipo l'id del tag non esiste

                            pizzaToUpdate.Tags.Add(tag);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                } else {
                    return NotFound("Il post che volevi modificare non è stato trovato!");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) {
            using (PizzaContext db = new PizzaContext()) {
                Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null) {
                    db.Pizze.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                } else {
                    return NotFound("La pizza da eliminare non è stata trovata!");
                }
            }
        }
    }
}
