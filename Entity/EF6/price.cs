namespace Entity.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("price")]
    public partial class price
    {
        public int id { get; set; }

        public int id_company { get; set; }

        [Column("price")]
        public double price1 { get; set; }

        public double firstChange { get; set; }

        public double lastChange { get; set; }

        public DateTime? lastUpdate { get; set; }
    }
}
