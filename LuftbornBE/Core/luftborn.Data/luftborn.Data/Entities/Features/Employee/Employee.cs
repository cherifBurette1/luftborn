using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luftborn.Data.Entities
{
    public class Employee : IEntityWithId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
