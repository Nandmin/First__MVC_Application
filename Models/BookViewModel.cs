using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_CRUD_app.Models
{
    public class BookViewModel
    {
        [Key]   // ezzel jelezzük, hogy a táblában ez a primary key
        public int BookId{ get; set; }
        [Required]      // ezzel jelezzük, hogy a mező értékét kötelező kitölteni
        public string Author{ get; set; }
        [Required]
        public string Title { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "0nál nagyobb érték kell")]          // ezzel lehet meghatározni, hogy a mező értéke mekkora tartományba essen
        public int Price { get; set; }
    }
}
