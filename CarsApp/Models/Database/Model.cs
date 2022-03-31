#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarsApp.Models.Database
{
    public partial class Model
    {
        public int Id { get; set; }

        [DisplayName("Марка")]
        public int BrandId { get; set; }
        [DisplayName("Модель")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Введите наименование модели")]
        [MaxLength(100, ErrorMessage ="Превышено допустимое кол-во символов (100)")]
        public string Name { get; set; }
        [DisplayName("Активен")]
        public bool IsActive { get; set; } = true;

        public virtual Brand Brand { get; set; }
    }
}
