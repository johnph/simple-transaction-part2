using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver.Service
{
    public static class Constants
    {
        public const string MediaTypeAppJson = "application/json";
    }

    public static class NamedHttpClients
    {
        public const string IdentityServiceClient = "idnsrvclient";
        public const string TransactionServiceClient = "txnsrvclient";
    }

    public class HttpConfigAttribs
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string MediaType { get; set; }
    }

    public class BaseUrlSettings
    {
        public string IdentityApiBaseUrl { get; set; }
        public string TransactionApiBaseUrl { get; set; }
    }

    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2
    }

    public enum Currency
    {
        Unknown = 0,

        [Description("United States dollar")]
        USD = 840,

        [Description("Pound sterling")]
        GBP = 826,

        [Description("Euro")]
        EUR = 978,

        [Description("Indian rupee")]
        INR = 356
    }

    public class CacheExpiry
    {
        public int ExpiryInSeconds { get; set; }
    }
}
