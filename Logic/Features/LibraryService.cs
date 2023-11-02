using Dal.Repositories;
using Dal.Models;
using Logic.Interfaces;

namespace Logic.Features
{
    public class LibraryService : ILibraryService
    {
        private readonly IGameDatabase _database;

        public LibraryService(IGameDatabase database)
        {
            _database = database;
        }

        public async Task<Game> CreateGame(Game game, List<string> genres)
        {
            var developer = await _database.FindOrCreateDeveloperAsync(game.DeveloperTitle);
            game.Developer = developer;
            var result = await _database.AddGameToDbAsync(game, genres);

            return result;
        }

        public async Task DeleteGame(int gameId)
        {
            var needed = await _database.FetchGameById(gameId);
            await _database.RemoveGameFromDbAsync(needed.Id);
        }

        public async Task<IEnumerable<Game>> FetchGames(IEnumerable<string>? genreFilter = null)
        {
            var games = await _database.FetchGamesAsync(genreFilter.ToList());

            return games;
        }

        public async Task<Game> UpdateGame(Game game)
        {
            var result = await _database.UpdateGameInDbAsync(game);

            return result;
        }

        public async Task<Game> FetchGameById(int id)
        {
            return await _database.FetchGameById(id);
        }
    }
}

