using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
ISubscriber subscriber = connection.GetSubscriber();

//pattern-matching
await subscriber.SubscribeAsync("mychannel.*", (channel, message) =>
{
    Console.WriteLine("listening...");
    Console.WriteLine(message);
});



//await subscriber.SubscribeAsync("mychannel", (channel, message) =>
//{
//    Console.WriteLine("listening mychannel...");
//    Console.WriteLine(message);
//});
Console.Read();