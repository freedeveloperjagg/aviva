using System;
using System.Collections.Generic;
using System.Text;

namespace AvivaLibrary.Models
{
    public class EntidadDePago
    {
        public int Id { get;  set; }
        public string Nombre { get; set; } = string.Empty;
        public List<Rule> Rules { get; set;  } = [];
    }
}
