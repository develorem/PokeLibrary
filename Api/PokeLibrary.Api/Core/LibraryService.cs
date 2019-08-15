using PokeLibrary.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeLibrary.Api.Core
{
    public interface ILibraryService
    {
        IEnumerable<PokemonCard> GetLibrary();
        PokemonCard Get(string id);

        Status AddToLibrary(string id, int count);
        Status RemoveFromLibrary(string id);
        Status Increment(string id, int count);
        Status Decrement(string id, int count);

        IEnumerable<PokemonCard> SearchName(string nameLike);
    }

    public class InMemoryLibraryService : ILibraryService
    {
        private readonly IPokemonCardService _pokemonCardService;

        private IList<OwnedCard> _library;

        public InMemoryLibraryService(IPokemonCardService pokemonCardService)
        {
            _pokemonCardService = pokemonCardService;
            _library = GetLibraryInternal().ToList();
        }

        private IEnumerable<OwnedCard> GetLibraryInternal()
        {
            yield return new OwnedCard { Id = "sm5-125", Count = 4 };
            yield return new OwnedCard { Id = "ex14-28", Count = 1 };
            yield return new OwnedCard { Id = "xy1-42", Count = 2 };
            yield return new OwnedCard { Id = "ex16-69", Count = 1 };
        }

        public IEnumerable<PokemonCard> GetLibrary()
        {
            var detailed = _library.Select(x => Get(x.Id)).ToArray();
            return detailed;
        }

        public PokemonCard Get(string id)
        {
            var card = _pokemonCardService.Get(id);
            DecorateCountOnCard(card);
            return card;
        }

        public Status AddToLibrary(string id, int count)
        {
            var existing = _library.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Count += count;
            }
            else
            {
                _library.Add(new OwnedCard { Id = id, Count = count });
            }
            return Status.Success;
        }
        
        public Status Increment(string id, int count)
        {
            var existing = _library.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Count += count;
                return Status.Success;
            }
            return Status.DoesNotExist;
        }

        public Status Decrement(string id, int count)
        {
            var existing = _library.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                if (existing.Count < count) return Status.InvalidParameter;

                existing.Count -= count;
                if (existing.Count <= 0)
                {
                    _library = _library.Where(x => x.Id != id).ToList();
                }
                return Status.Success;
            }
            return Status.DoesNotExist;
        }

        public Status RemoveFromLibrary(string id)
        {
            var existing = _library.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                _library = _library.Where(x => x.Id != id).ToList();
                return Status.Success;
            }
            return Status.DoesNotExist;
        }

        public IEnumerable<PokemonCard> SearchName(string nameLike)
        {
            var results = _pokemonCardService.Search(nameLike);
            foreach (var card in results)
            {
                DecorateCountOnCard(card);
            }
            return results;
        }

        private void DecorateCountOnCard(PokemonCard card)
        {
            var match = _library.FirstOrDefault(x => x.Id == card.Id);
            if (match != null) card.Count = match.Count;
        }
    }

    public enum Status
    {
        Success,
        DoesNotExist,
        InvalidParameter,
        UnknownError
    }
}
