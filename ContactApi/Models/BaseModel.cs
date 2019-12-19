using System.ComponentModel.DataAnnotations;

namespace ContactApi.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}