using Dal.Models;
using Dal.Interfaces;

namespace Logic.Interfaces
{
    public interface ILibraryService
    {
        public Task<Game> CreateGame(Game game, List<string> genres);
        public Task DeleteGame(int gameId);
        public Task<Game> UpdateGame(int id, IPublicGame updatedGame);
        public Task<IEnumerable<Game>> FetchGames(IEnumerable<string>? genreFilter = null);
        public Task<Game> FetchGameById(int id);
    }
}