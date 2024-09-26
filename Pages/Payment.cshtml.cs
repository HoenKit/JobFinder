using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS.Types;
using Net.payOS;
using JobFinder.Models.PayOs;

namespace JobFinder.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly PayOS _payOS;
        private readonly ILogger<PaymentModel> _logger;

        public PaymentModel(PayOS payOS, ILogger<PaymentModel> logger)
        {
            _payOS = payOS;
            _logger = logger;
        }

        [BindProperty]
        public CreatePaymentLinkRequest PaymentRequest { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostCreatePaymentLinkAsync()
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(PaymentRequest.ProductName, 1, 30000);
                List<ItemData> items = new List<ItemData> { item };
                PaymentData paymentData = new PaymentData(orderCode, 30000, PaymentRequest.Description, items, PaymentRequest.CancelUrl, PaymentRequest.ReturnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Redirect(createPayment.checkoutUrl); 
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error creating payment link");
                ModelState.AddModelError(string.Empty, "Lỗi khi tạo yêu cầu thanh toán.");
                return Page();
            }
        }

    }
}
