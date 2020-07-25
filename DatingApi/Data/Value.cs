using System.ComponentModel.DataAnnotations;
namespace Data
{
    public class Value
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}