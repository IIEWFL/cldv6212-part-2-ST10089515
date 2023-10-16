using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=sakopano;AccountKey=0uJk9x/GShmzpqNjEigFSH5NXps5BMkAv13XTlreMmAV9IS/q9l97jHBU2ig7kPmdCm9OekazXYX+AStfC5QXw==;EndpointSuffix=core.windows.net";
           
        string queueName = "messages";

        // Create a QueueClient to interact with the queue.
        QueueClient queueClient = new QueueClient(connectionString, queueName);

        // Create the queue if it doesn't exist.
        await queueClient.CreateAsync();

        // Get user input for ID
        Console.WriteLine("Enter ID number:");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID input.");
            return;
        }

        // Get user input for Vaccination Center
        Console.WriteLine("Enter Vaccination Center:");
        string center = Console.ReadLine();

        // Get user input for Date
        Console.WriteLine("Enter Date:");
        string date = Console.ReadLine();

        // Get user input for Vaccine Serial Number
        Console.WriteLine("Enter Vaccine Serial Number:");
        string serial = Console.ReadLine();

        // Get user input for Vaccine Barcode
        Console.WriteLine("Enter Vaccine Barcode:");
        string barcode = Console.ReadLine();

        // Example messages (Id:VaccinationCenter:VaccinationDate:VaccineSerialNumber)
        string message1 = $"{id}:{center}:{date}:{serial}";
        string message2 = $"{barcode}:{date}:{center}:{id}";

        // Add messages to the queue.
        await SendMessage(queueClient, message1);
        await SendMessage(queueClient, message2);

        Console.WriteLine("Messages added to the queue.");
    }

    static async Task SendMessage(QueueClient queueClient, string message)
    {
        try
        {
            await queueClient.SendMessageAsync(message);
            Console.WriteLine($"Message sent: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }
}
