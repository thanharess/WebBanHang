using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanHang.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)]")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; } // Số lượng tồn kho

        public string ImageUrl { get; set; } // Đường dẫn ảnh sản phẩm

        public string Description { get; set; }

        // Khóa ngoại liên kết tới bảng Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}