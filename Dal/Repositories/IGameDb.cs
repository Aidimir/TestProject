using System;
using Dal.Models;

namespace Dal.Repositories
{
	public interface IGameDatabase
	{
        public Task<Game> AddGameToDbAsync(Game game, List<string> genres);
        public Task<Game> UpdateGameInDbAsync(Game game);
        public Task RemoveGameFromDbAsync(int id);
        public Task<IEnumerable<Game>> FetchGamesAsync(List<string>? genreFilter = null);
        public Task<Game> FetchGameById(int id);
        public Task<Developer> FindOrCreateDeveloperAsync(string title);
    }
}