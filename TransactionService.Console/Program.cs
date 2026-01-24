// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Demonstrating the difference between class, struct, and record in C#
        var moneyClass1 = new MoneyClass { Amount = 100 };
        var moneyClass2 = moneyClass1;
        moneyClass2.Amount = 200;

        Console.WriteLine($"MoneyClass1 Amount: {moneyClass1.Amount}"); // Outputs 200
        Console.WriteLine($"MoneyClass2 Amount: {moneyClass2.Amount}"); // Outputs 200

        var moneyStruct1 = new MoneyStruct { Amount = 100 };
        var moneyStruct2 = moneyStruct1;
        moneyStruct2.Amount = 200;
        Console.WriteLine($"MoneyStruct1 Amount: {moneyStruct1.Amount}"); // Outputs 100
        Console.WriteLine($"MoneyStruct2 Amount: {moneyStruct2.Amount}"); // Outputs 200

        var moneyRecord1 = new MoneyRecord(100, "USD");
        var moneyRecord2 = moneyRecord1 with { Amount = 200, Currency = "AUD" };
        Console.WriteLine($"MoneyRecord1 Amount: {moneyRecord1.Amount} {moneyRecord1.Currency}"); // Outputs 100 USD
        Console.WriteLine($"MoneyRecord2 Amount: {moneyRecord2.Amount} {moneyRecord2.Currency}"); // Outputs 200 USD

        // async await example
        var t = GetStatusAsync();
        Console.WriteLine("Doing other work while waiting for async operation...");
        var status = await t;
        Console.WriteLine($"Async operation status: {status}");

        // cpu bound async example
        var cpuBoundTask = Task.Run(() =>
        {
            long sum = 0;
            for (long i = 0; i < 1_000_000_000; i++)
            {
                sum += i;
            }
            return sum;
        });

        Console.WriteLine("Doing other work while waiting for CPU-bound operation...");
        var cpuBoundResult = await cpuBoundTask;
        Console.WriteLine($"CPU bound result: {cpuBoundResult}");

        // IAsyncEnumerable example
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            await foreach (var txId in StreamTransactionsAsync(cts.Token))
            {
                Console.WriteLine($"Received transaction ID: {txId}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Transaction streaming was canceled.");
        }

        // delegates example
        Func<int, int, int> add = (a, b) =>
        {
            return a + b;
        };

        int result = add(5, 10);
        Console.WriteLine($"Delegate result: {result}");

    }

    static async IAsyncEnumerable<int> StreamTransactionsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(500); // Simulate delay
            yield return new Random().Next(1, 100); // Simulate transaction ID
        }


    }

    private static async Task<string> GetStatusAsync()
    {
        Console.WriteLine("Starting async operation...");
        await Task.Delay(1000); // Simulate some asynchronous work
        Console.WriteLine("Completed async operation...");
        return "Completed";
    }


}

public class MoneyClass
{
    public long Amount;
}

public struct MoneyStruct
{
    public long Amount;
}

public record MoneyRecord(long Amount, string Currency);
