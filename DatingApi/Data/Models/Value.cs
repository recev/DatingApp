using System.ComponentModel.DataAnnotations;
namespace  DatingApi.Data.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}