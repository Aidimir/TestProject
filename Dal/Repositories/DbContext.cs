using Microsoft.EntityFrameworkCore;
using Dal.Repositories;
using Dal.Models;
using Dal.Exceptions;

namespace Dal.Repositories
{
    public class GameDatabase : DbContext, IGameDatabase
    {
        DbSet<Game> games { get; set; }

        DbSet<Developer> developers { get; set; }

        public GameDatabase(DbContextOptions options) : base(options) { }

        public async Task<IEnumerable<Game>> FetchGamesAsync(List<string>? genreFilter)
        {
            if (genreFilter == null || genreFilter.Count() == 0)
            {
                return await games.ToListAsync();
            }
            var loweredCaseFilter = genreFilter.Select(x => x.ToLower());
            var listGames = await games.ToListAsync();

            return listGames.Where(game => game.Genre.Select(x => x.ToLower()).Intersect(loweredCaseFilter).Count() != 0);
        }

        public async Task<Game> AddGameToDbAsync(Game game)
        {
            var sameGameInDb = await games.FirstOrDefaultAsync(g => g.Title == game.Title && g.Developer == game.Developer);
            if (sameGameInDb != null)
            {
                throw new ObjectAlreadyExistsException("This game is already in database");
            }
            await games.AddAsync(game);
            await SaveChangesAsync();

            return game;
        }

        public async Task<Game> UpdateGameInDbAsync(Game game)
        {
            games.Update(game);
            await SaveChangesAsync();

            return game;
        }

        public async Task<Game> FetchGameById(int id)
        {
            var result = await games.FindAsync(id);
            if (result == null)
            {
                throw new NotFoundException("Couldn't find any game with this id");
            }

            return result;
        }

        public async Task RemoveGameFromDbAsync(int id)
        {
            var neededGame = await FetchGameById(id);
            games.Remove(neededGame);
            await SaveChangesAsync();
        }

        public async Task<Developer> FindOrCreateDeveloperAsync(string title)
        {
            var devInDb = await developers.FirstOrDefaultAsync(g => g.Title.ToLower() == title.ToLower());
            if (devInDb == null)
            {
                var newDeveloper = new Developer { Title = title };
                await developers.AddAsync(newDeveloper);
                await SaveChangesAsync();
                return newDeveloper;
            }
            else
            {
                return devInDb;
            }
        }
    }
}