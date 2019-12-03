using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var input = new HelloRequest() { Name = "Bob" };
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(input);

            //Console.WriteLine(reply.Message);

            var input = new CustomerLookupModel() { UserId = 2 };
            
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customer.CustomerClient(channel);
            
            var customer = await customerClient.GetCustomerInfoAsync(input);

            Console.WriteLine($"{customer.FirstName} {customer.LastName}");
            
            Console.WriteLine();
            Console.WriteLine("new customer list");
            Console.WriteLine();

            using (var call = customerClient.GetNewCustomers(new NewCustomersRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} - {currentCustomer.EmailAddress}");
                }
            }

            Console.ReadLine();
        }
    }
}
