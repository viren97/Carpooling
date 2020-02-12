using Carpooling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carpooling.DataProvider.DataProviderInterfaces {
    public interface IPaymentRepository {
        void AddPayment(Payment payment);
        Payment GetPaymentById(int id);

    }
}
