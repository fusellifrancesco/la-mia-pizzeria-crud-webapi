namespace La_mia_pizzeria_1_n.Models {
    public class Category {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Pizza> Pizze { get; set; }

        public Category() {

        }
    }
}
