using System.ComponentModel.DataAnnotations;

namespace ReadingIsGood.Domain.Entity
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}