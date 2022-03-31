using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CarsApp.Models.Database
{
    public partial class Brand
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public int Id { get; set; }
        [DisplayName("Марка")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите наименование марки")]
        [MaxLength(100, ErrorMessage = "Превышено допустимое кол-во символов (100)")]
        public string Name { get; set; }

        [DisplayName("Активен")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Модели")]
        public virtual ICollection<Model> Models { get; set; }
    }
}
