using System.Text.Json.Serialization;

namespace La_mia_pizzeria_1_n.Models {
    public class Tag {

        public int Id { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public List<Pizza>? Pizze { get; set; }

        public Tag() {

        }

    }
}
