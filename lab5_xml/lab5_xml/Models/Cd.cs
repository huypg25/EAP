

using System.ComponentModel.DataAnnotations;
namespace lab5_xml.Models
{
    public class Cd
    {
        [Required(ErrorMessage = "Hãy nhập tiêu đề")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Hãy nhập nghệ sĩ")]
        public string Artist { get; set; }
        [Required(ErrorMessage = "Hãy nhập đất nước")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Hãy nhập công ty phát hành")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Hãy nhập giá")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Hãy nhập năm xuất bản")]
        public int Year { get; set; }
    }
}