using Microsoft.EntityFrameworkCore;
using Dal.Models;
using Dal.Exceptions;

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
            modelBuilder.Entity<GameGenre>()
    .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId);

        }

        public async Task<IEnumerable<Game>> FetchGamesAsync(List<string>? genreFilter)
        {
            if (genreFilter == null || genreFilter.Count() == 0)
            {
                return await _games.Include(x => x.GameGenres).ThenInclude(gg => gg.Genre).Include(x => x.Developer).ToListAsync();
            }

            var filteredGames = await _games
                .Where(game => game.GameGenres.Any(gg => genreFilter.Contains(gg.Genre.Name)))
                .Include(game => game.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(game => game.Developer)
                .ToListAsync();
            return filteredGames;
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
                game.GameGenres.Add(new GameGenre { Game = game, Genre = g });
            });

            var developer = await FindOrCreateDeveloperAsync(game.DeveloperTitle);
            developer.Games.Add(game);
            _developers.Update(developer);

            await SaveChangesAsync();

            return  await _games.Include(x => x.GameGenres).Include(x => x.Developer).Where(x => x.Id == game.Id).FirstAsync();
        }

        public async Task<Game> UpdateGameInDbAsync(Game game)
        {
            _games.Update(game);
            await SaveChangesAsync();

            return game;
        }

        public async Task<Game> FetchGameById(int id)
        {
            var result = await _games.FindAsync(id);
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
            var notInDb = genres.Where(y => !_genres.Select(x => x.Name.ToLower()).Contains(y.ToLower())).Select(x => new Genre { Name = x }).ToList();
            notInDb.ForEach(async (obj) => await _genres.AddAsync(obj));
            await SaveChangesAsync();

            return await _genres.Where(x => genres.Select(y => y.ToLower()).Contains(x.Name.ToLower())).ToListAsync();
        }
    }
}