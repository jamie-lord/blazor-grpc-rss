using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcItems;

namespace Rss.Server.Services
{
    public class ItemService : Items.ItemsBase
    {
        public override async Task<Reply> GetItems(Empty request, ServerCallContext context)
        {
            Feed feed = await FeedReader.ReadAsync("https://www.theguardian.com/world/rss");
            var reply = new Reply();
            foreach (FeedItem item in feed.Items)
            {
                reply.Items.Add(new Item
                {
                    Link = item.Link,
                    Title = item.Title
                });
            }
            return reply;
        }
    }
}
