using Microsoft.AspNetCore.Mvc.Rendering;

namespace La_mia_pizzeria_1_n.Models {
    public class PizzaCategoriesView {
        
        public Pizza Pizza { get; set; }

        public List<Category>? Categories { get; set; }

        // Questa proprietà ci servirà per poter passare alla vista che contiene il form
        // la lista di tutti i tag da cui l'utente potrà selezionare
        public List<SelectListItem>? Tags { get; set; }

        // Predisponiamo il nostro modello per la vista Create e MOdify con questa nuova proprietà per poter
        // ricevere gli id dei tag che l'utente ha selezionato. Purtroppo questi saranno di tipo string perchè quando avevamo
        // passato i Value per ogni opzione nella select questi erano di tipo string!
        public List<string>? TagsSelectedFromMultipleSelect { get; set; }
    }
}
