using Newtonsoft.Json;
using PokeLibrary.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PokeLibrary.Api.Core
{
    public interface IPokemonCardService
    {
        PokemonCard Get(string id);
        IEnumerable<PokemonCard> Search(string nameLike);
    }

    public class PokemonCardService : IPokemonCardService
    {
        private readonly string _urlBase;

        public PokemonCardService()
        {
            _urlBase = "https://api.pokemontcg.io/v1/";
        }

        public PokemonCard Get(string id)
        {
            var fullurl = $"{_urlBase}cards/{id}";
            var result =  Execute<SingleCardResult>(fullurl);
            return result.Card;
        }

        public IEnumerable<PokemonCard> Search(string nameLike)
        {
            var name = HttpUtility.UrlEncode(nameLike);
            var fullUrl = $"{_urlBase}cards?name={name}";

            var result = Execute<MultipleCardsResult>(fullUrl);
            return result.Cards;
        }

        private T Execute<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(data);
                }
            }

            throw new ArgumentException("Url must have been bad: " + url);
        }
    }
}
