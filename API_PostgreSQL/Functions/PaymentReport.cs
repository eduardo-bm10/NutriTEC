namespace Postgre_API.Functions {
    public class PaymentReport {
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? TotalPayment { get; set; }
        public string Discount { get; set; }
        public float? FinalPayment { get; set; }
    }
}