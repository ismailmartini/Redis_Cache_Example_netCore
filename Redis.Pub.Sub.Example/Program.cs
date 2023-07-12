using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();

while (true)
{
    Console.Write("Publish a New Message : ");
    string message = Console.ReadLine();
    await subscriber.PublishAsync("mychannel", message);
}