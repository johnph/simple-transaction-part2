namespace SimpleBanking.ConsoleApp
{
    using Newtonsoft.Json;
    using SimpleBanking.ConsoleApp.Extension;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static SimpleBanking.ConsoleApp.Models;

    class Program
    {
        static readonly string baseUrl = "http://localhost:54784";
        static HttpClient client = new HttpClient();

        static async Task<SecurityToken> Authenticate(Login login)
        {
            var response = await client.PostAsJsonAsync($"/user/authenticate", new { Username = login.UserName, Password = login.Password });
            var token = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SecurityToken>(token);
        }

        static async Task<TransactionResult> BalanceAsync()
        {
            var response = await client.GetAsync($"/account/balance");
            return await DeserializeResponseContent(response);
        }

        static async Task<TransactionResult> DepositAsync(TransactionInput transactionDetails)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/account/deposit", transactionDetails);
            return await DeserializeResponseContent(response);
        }

        static async Task<TransactionResult> WithdrawAsync(TransactionInput transactionDetails)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/account/withdraw", transactionDetails);
            return await DeserializeResponseContent(response);
        }

        static async Task<StatementResult> StatementAsync(string month)
        {
            var response = await client.GetAsync($"/statement/{month}");
            return await DeserializeResponseContent2(response);
        }

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }        

        static async Task RunAsync()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Simple Transaction Processing");

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var login = ReadLoginDetails();
                var accessToken = await Authenticate(login);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.auth_token);
                Console.WriteLine("\n Login Successfull.");

                //var data = GetAllTransactionData();

                //foreach (var item in data)
                //{
                //    try
                //    {
                //        await MakeTransaction(item);
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}

                DisplayMenu();

                string key;
                while ((key = Console.ReadKey().KeyChar.ToString()) != "5")
                {
                    int.TryParse(key, out int keyValue);

                    switch (keyValue)
                    {
                        case 1:
                            await ShowBalance();
                            break;
                        case 2:
                            await MakeTransaction(TransactionType.Deposit);
                            break;
                        case 3:
                            await MakeTransaction(TransactionType.Withdrawal);
                            break;
                        case 4:
                            await ShowStatement();
                            break;
                    }

                    Console.Write("Enter the option (number): ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("App interrupted.");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("App closed.");
            }            

            Console.ReadLine();
        }

        static string ReadAccountNumber()
        {
            Console.WriteLine();
            Console.Write("Enter the account Number: ");
            var accountNumber = Console.ReadLine();
            return accountNumber;
        }

        static Login ReadLoginDetails()
        {
            Console.WriteLine();
            Console.Write("Enter the user name: ");
            var username = Console.ReadLine();
            Console.Write("Enter the password: ");
            var password = Console.ReadLine();
            return new Login() { UserName = username, Password = password };
        }

        static void DisplayMenu()
        {            
            Console.WriteLine();
            Console.WriteLine("1. Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Statement");
            Console.WriteLine("5. Close app (X)");
            Console.WriteLine();
            Console.Write("Enter the option (number): ");
        }

        static async Task ShowBalance()
        {
            var transactionResult = await BalanceAsync();

            Console.WriteLine();
            Console.WriteLine("Balance");
            Console.WriteLine();

            if(transactionResult.Balance != null)
            {
                Console.WriteLine($"Account No: {transactionResult.AccountNumber}");
                Console.WriteLine($"Balance: {transactionResult.Balance}");
                Console.WriteLine($"Currency: {transactionResult.Currency}");
            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
                Console.WriteLine($"Message: {transactionResult.Message}");
            }

            Console.WriteLine();
        }

        static async Task ShowStatement()
        {
            Console.WriteLine();
            Console.Write("Enter the Month ('MMM-yyyy'): ");
            var month = Console.ReadLine();

            var statementResult = await StatementAsync(month);

            Console.WriteLine();
            Console.WriteLine($"Statement for {month}");
            Console.WriteLine();

            if (statementResult.AccountNumber != null)
            {
                Console.WriteLine($"Name: {statementResult.Name}");
                Console.WriteLine($"Account No: {statementResult.AccountNumber}");                
                Console.WriteLine($"Currency: {statementResult.Currency}");
                
                Console.WriteLine(statementResult.TransactionDetails.ToStringTable<AccountTransaction>(
                    new[] { "Date", "Description", "Withdrawal", "Deposit", "Balance" },
                    a => a.Date, a => a.TransactionDetail, a => a.Withdrawal, a => a.Deposit, a => a.Balance));
            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
                Console.WriteLine($"Message: {statementResult.Message}");
            }

            Console.WriteLine();
        }

        static async Task MakeTransaction(TransactionType transactionType)
        {
            Console.WriteLine();

            Console.Write("Enter the Amount: ");
            var transactionAmount = Console.ReadLine();

            Console.Write("Enter the description: ");
            var description = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine(transactionType.ToString());
            Console.WriteLine();

            var transactionInput = new TransactionInput() {
                TransactionType = transactionType,
                Amount = Math.Round(Convert.ToDecimal(transactionAmount), 2),
                Description = description,
                Date = DateTime.UtcNow
            };

            var transactionResult = new TransactionResult();
            if (transactionType == TransactionType.Deposit)
            {
                transactionResult = await DepositAsync(transactionInput);
            }
            else if(transactionType == TransactionType.Withdrawal)
            {
                transactionResult = await WithdrawAsync(transactionInput);
            }

            if(transactionResult.IsSuccessful)
            {
                Console.WriteLine($"Status: {transactionResult.Message}");
                Console.WriteLine($"Account No: {transactionResult.AccountNumber}");
                Console.WriteLine($"Current Balance: {transactionResult.Balance}");
                Console.WriteLine($"Currency: {transactionResult.Currency}");                                
            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
                Console.WriteLine($"Message: {transactionResult.Message}");               
            }

            Console.WriteLine();
        }

        static async Task MakeTransaction(TransactionData transactionData)
        {
            var transactionType = transactionData.Deposits > decimal.Zero ? TransactionType.Deposit : TransactionType.Withdrawal;
            var transactionAmount = transactionData.Deposits > decimal.Zero ? transactionData.Deposits : transactionData.Withdrawals;

            var transactionInput = new TransactionInput()
            {
                Date = transactionData.Date,
                TransactionType = transactionType,
                Amount = Convert.ToDecimal(transactionAmount),
                Description = transactionData.TransactionDetails
            };

            var transactionResult = new TransactionResult();
            if (transactionType == TransactionType.Deposit)
            {
                transactionResult = await DepositAsync(transactionInput);
            }
            else if (transactionType == TransactionType.Withdrawal)
            {
                transactionResult = await WithdrawAsync(transactionInput);
            }

            if (transactionResult.IsSuccessful)
            {
                Console.WriteLine($"Status: {transactionResult.Message}");
                Console.WriteLine($"Account No: {transactionResult.AccountNumber}");
                Console.WriteLine($"Current Balance: {transactionResult.Balance}");
                Console.WriteLine($"Currency: {transactionResult.Currency}");
            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
                Console.WriteLine($"Message: {transactionResult.Message}");
            }

            Console.WriteLine();
        }

        static async Task<TransactionResult> DeserializeResponseContent(HttpResponseMessage response)
        {
            var transactionResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransactionResult>(transactionResult);
        }

        static async Task<StatementResult> DeserializeResponseContent2(HttpResponseMessage response)
        {
            var transactionResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StatementResult>(transactionResult);
        }

        static List<TransactionData> GetAllTransactionData()
        {
            // read the excel and prepare the dataset
            string csvFilePath = @"C:\sampledata\sample-data.csv";
            var transactionData = new List<TransactionData>();
            var lines = File.ReadAllLines(csvFilePath);
            transactionData = lines.Skip(1).Select(v => FromCsv(v)).Where(i => i != null).OrderBy(e => e.Date).ToList();
            return transactionData;
        }

        static TransactionData FromCsv(string csvLine)
        {
            try
            {
                string[] values = csvLine.Split(',');
                TransactionData data = new TransactionData();
                data.Date = DateTime.ParseExact(values[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                data.TransactionDetails = values[1];
                data.Withdrawals = string.IsNullOrEmpty(values[2]) ? decimal.Zero : Math.Round(Convert.ToDecimal(values[2]));
                data.Deposits = string.IsNullOrEmpty(values[3]) ? decimal.Zero : Math.Round(Convert.ToDecimal(values[3]), 2);
                data.Balance = Math.Round(Convert.ToDecimal(values[4]), 2);
                return data;
            }
            catch (Exception)
            {
                return null;
            }            
        }
    }
}
