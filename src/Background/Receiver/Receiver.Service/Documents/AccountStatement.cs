namespace Receiver.Service.Documents
{
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class AccountStatement
    {
        [BsonId]
        public string Key { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("accountnumber")]
        public int AccountNumber { get; set; }
        [BsonElement("currency")]
        public string Currency { get; set; }
        [BsonElement("startdate")]
        public string StartDate { get; set; }
        [BsonElement("enddate")]
        public string EndDate { get; set; }
        [BsonElement("openingbalance")]
        public decimal? OpeningBalance { get; set; }
        [BsonElement("closingbalance")]
        public decimal? ClosingBalance { get; set; }
        [BsonElement("transactions")]
        public IEnumerable<AccountTransaction> TransactionDetails { get; set; }
    }

    public class AccountTransaction
    {
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("description")]
        public string TransactionDetail { get; set; }
        [BsonElement("withdrawal")]
        public string Withdrawal { get; set; }
        [BsonElement("deposit")]
        public string Deposit { get; set; }
        [BsonElement("balance")]
        public decimal Balance { get; set; }
    }
}
