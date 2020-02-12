using Carpooling.DataProvider.DataProviderInterfaces;
using Carpooling.Models;

namespace Carpooling.DataProvider.DataProviders {
    public class PaymentRepository : IPaymentRepository {
        private CarpoolingContext CarpoolContext = new CarpoolingContext();
        public void AddPayment(Payment payment) {
            CarpoolContext.Payments.Add(payment);
            CarpoolContext.SaveChanges();
        }

        public Payment GetPaymentById(int id) {
            return CarpoolContext.Payments.Find(id);
        }

    }
}
