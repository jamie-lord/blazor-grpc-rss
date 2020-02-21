using System;
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
            foreach (FeedItem feedItem in feed.Items)
            {
                var item = new Item
                {
                    Link = feedItem.Link,
                    Title = feedItem.Title,
                    PublishDate = feedItem.PublishingDate.HasValue ? feedItem.PublishingDate.Value.ToTimestamp() : DateTime.Now.ToTimestamp()
                };
                if (!string.IsNullOrWhiteSpace(feedItem.Author))
                {
                    item.Author = feedItem.Author;
                }
                if (!string.IsNullOrWhiteSpace(feedItem.Content))
                {
                    item.Content = feedItem.Content;
                }
                reply.Items.Add(item);
            }
            return reply;
        }
    }
}
