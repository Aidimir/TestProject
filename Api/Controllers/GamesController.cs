using Microsoft.AspNetCore.Mvc;
using Dal.Models;
using Microsoft.AspNetCore.JsonPatch;
using Logic.Interfaces;
using Api.Controllers.DTO.RequestModels;
using Api.Controllers.DTO.ResponseModels;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILibraryService _service;

        public GamesController(ILibraryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates new game in database
        /// </summary>
        /// <response code="201">Game created succesfully</response>
        /// <param name="request">Your game-create request</param>
        /// <returns>Returns just created game object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponseModel))]
        public async Task<ActionResult<Game>> Create(CreateGameRequestModel request)
        {
            var game = new Game { DeveloperTitle = request.Developer, Genre = request.Genre, Title = request.Title };
            var createdGame = await _service.CreateGame(game);

            return StatusCode(201, createdGame);
        }

        /// <summary>
        /// Retrieves games from database
        /// </summary>
        /// <response code="200">Games retreived succesfully</response>
        /// <param name="genres">List of genre strings, can be null or contain some values. If database has no such genres it will return empty list of games</param>
        /// <returns>Returns a list of games which satisfies to given parameters (if parameters is null or empty => just returns all games in db)</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponseModel))]
        public async Task<ActionResult<IEnumerable<Game>>> FetchGames([FromQuery] IEnumerable<string>? genres)
        {
            var games = await _service.FetchGames(genres);

            return StatusCode(200, games.ToList());
        }

        /// <summary>
        /// Updates game property in database
        /// </summary>
        /// <param name="gameId">Unique integer id of the game</param>
        /// <param name="gameModel">Patch model of the game</param>
        /// <response code="202">Game updated succesfully</response>
        /// <response code="404">Didn't find any game in database</response>
        /// <response code="400">Bad request</response>

        ///<remarks>
        ///Sample request:
        ///
        ///     PATCH
        ///     {
        ///         {
        ///             "path": "Title",
        ///             "op": "replace",
        ///             "value": "My new title"
        ///         },
        ///         {
        ///             "path": "Genre",
        ///             "op": "replace",
        ///             "value": "["Cringe horror"]"
        ///         }
        ///     }
        ///</remarks>

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Game))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponseModel))]
        public async Task<ObjectResult> Update(int gameId, [FromBody] JsonPatchDocument<Game> gameModel)
        {
            var neededGame = await _service.FetchGameById(gameId);
            gameModel.ApplyTo(neededGame, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if (!TryValidateModel(neededGame))
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).SelectMany(error => error.ErrorMessage);
                var errorsString = new string(errors.ToArray());
                throw new FormatException($"One or more validation parameters are failed: {ModelState.ErrorCount}." +
                    $" Errors: {errorsString}");
            }
            var updatedGame = await _service.UpdateGame(neededGame);

            return StatusCode(200, updatedGame);
        }

        /// <summary>
        /// Removes game from database 
        /// </summary>
        /// <param name="gameId">Unique integer id of the game</param>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponseModel))]
        public async Task<NoContentResult> Delete(int gameId)
        {
            await _service.DeleteGame(gameId);

            return NoContent();
        }
    }
}