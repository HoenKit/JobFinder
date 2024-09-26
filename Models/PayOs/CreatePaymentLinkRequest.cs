﻿namespace JobFinder.Models.PayOs
{
    public class CreatePaymentLinkRequest
    {
        // Tên sản phẩm
        public string ProductName { get; set; }

        // Mô tả sản phẩm
        public string Description { get; set; }

        // Giá sản phẩm
        public int Price { get; set; }

        // URL để trở về sau khi thanh toán
        public string ReturnUrl { get; set; } = "https://localhost:7292/Identity/Account/RegisterRecruiter";

        // URL để hủy thanh toán
        public string CancelUrl { get; set; } = "https://localhost:7292/Identity/Account/Register";
    }
}