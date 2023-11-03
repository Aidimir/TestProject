using Microsoft.EntityFrameworkCore;
using Dal.Models;
using Dal.Exceptions;
using Dal.Interfaces;
using System;

namespace Dal.Repositories
{
    public class GameDatabase : DbContext, IGameDatabase
    {
        DbSet<Game> _games { get; set; }

        DbSet<Developer> _developers { get; set; }

        DbSet<Genre> _genres { get; set; }

        public GameDatabase(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(c => c.Genres)
                .WithMany(s => s.Games)
                .UsingEntity(j => j.ToTable("GameGenres"));
        }

        public async Task<IEnumerable<Game>> FetchGamesAsync(List<string>? genreFilter)
        {
            if (genreFilter == null || genreFilter.Count() == 0)
            {
                return await _games.Include(x => x.Genres).Include(x => x.Developer).ToListAsync();
            }

            return await _genres
                .Where(x => genreFilter.Select(str => str.ToLower())
                .Contains(x.Name.ToLower()))
                .SelectMany(genre => genre.Games)
                .Include(x => x.Developer)
                .Include(x => x.Genres)
                .ToListAsync();
        }

        public async Task<Game> AddGameToDbAsync(Game game, List<string> genres)
        {
            var sameGameInDb = await _games.FirstOrDefaultAsync(g => g.Title.ToLower() == game.Title.ToLower() && g.Developer == game.Developer);
            if (sameGameInDb != null)
            {
                throw new ObjectAlreadyExistsException("This game is already in database");
            }

            await _games.AddAsync(game);

            var genresFromDb = await FindOrCreateGenresAsync(genres);
            genresFromDb.ForEach(g =>
            {
                game.Genres.Add(g);
                g.Games.Add(game);
            });

            var developer = await FindOrCreateDeveloperAsync(game.DeveloperTitle);
            developer.Games.Add(game);

            await SaveChangesAsync();

            return await _games
                .Include(x => x.Genres)
                .Include(x => x.Developer)
                .Where(x => x.Id == game.Id)
                .FirstAsync();
        }

        public async Task<Game> UpdateGameInDbAsync(int id, IPublicGame updatedGame)
        {
            var existingGame = await FetchGameById(id);
            var existingDeveloper = await FindOrCreateDeveloperAsync(updatedGame.Developer);
            var existingGenres = await FindOrCreateGenresAsync(updatedGame.Genres);

            existingGame.Title = updatedGame.Title;
            existingGame.Developer = existingDeveloper;
            existingGame.Genres = existingGenres;

            await SaveChangesAsync();

            return existingGame;
        }

        public async Task<Game> FetchGameById(int id)
        {
            var result = await _games
                .Include(x => x.Developer)
                .Include(x => x.Genres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new NotFoundException("Couldn't find any game with this id");
            }

            return result;
        }

        public async Task RemoveGameFromDbAsync(int id)
        {
            var neededGame = await FetchGameById(id);
            _games.Remove(neededGame);
            await SaveChangesAsync();
        }

        public async Task<Developer> FindOrCreateDeveloperAsync(string title)
        {
            var devInDb = await _developers.FirstOrDefaultAsync(g => g.Title.ToLower() == title.ToLower());
            if (devInDb == null)
            {
                var newDeveloper = new Developer { Title = title };
                await _developers.AddAsync(newDeveloper);
                await SaveChangesAsync();
                return newDeveloper;
            }
            else
            {
                return devInDb;
            }
        }

        public async Task<List<Genre>> FindOrCreateGenresAsync(List<string> genres)
        {
            var notInDb = genres
                .Select(y => y.ToLower())
                .Except(_genres.Select(x => x.Name.ToLower()))
                .Select(x => new Genre { Name = x })
                .ToList();

            await _genres.AddRangeAsync(notInDb);
            await SaveChangesAsync();

            return await _genres
                .Where(x => genres.Select(y => y.ToLower())
                .Contains(x.Name.ToLower()))
                .ToListAsync();
        }
    }
}