using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using async_bgg.model.bgg;

namespace async_bgg
{
    public class BggClient
    {
        private const string BoardGamesGeekApiEndpointTemplate = 
            "https://www.boardgamegeek.com/xmlapi2/collection?username={0}&own=1&brief=1&stats=1";

        private static readonly HttpClient client = new();

        public async Task<CollectionBgg> DownloadUserCollectionAsync(string username)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            var streamTask = client.GetStreamAsync(string.Format(BoardGamesGeekApiEndpointTemplate, username));
            var serializer = new XmlSerializer(typeof(CollectionBgg));
            return (CollectionBgg) serializer.Deserialize(await streamTask);
        }
    }
}