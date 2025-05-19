namespace ProyectoCatedraPED.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Score")]
    public partial class Score
    {
        public int id { get; set; }

        public int book_id { get; set; }

        public int genre_id { get; set; }

        [Column("score")]
        public decimal score1 { get; set; }

        public virtual Book Book { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
