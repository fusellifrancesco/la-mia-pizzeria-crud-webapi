using La_mia_pizzeria_1_n.Database;
using La_mia_pizzeria_1_n.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace La_mia_pizzeria_1_n.Utils {
    public static class TagsConverter {

        public static List<SelectListItem> getListTagsForMultipleSelect() {
            using (PizzaContext db = new PizzaContext()) {
                List<Tag> tagsFromDb = db.Tags.ToList<Tag>();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Tag che provengono da Db
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Tag tag in tagsFromDb) {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }
    }
}
