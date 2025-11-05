using DnD.Domain.Services;

namespace DnD.Api.Endpoints
{
    public static class PlayerEndpoints
    {
        public static void MapPlayerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/players").WithTags("Players");

            group.MapGet("/", async (IPlayerService service) =>
            {
                return Results.Ok(await service.GetAllPlayersAsync());
            }).WithSummary("Recupera tutti i giocatori");

            group.MapGet("/{id:int}", async (int id, IPlayerService service) =>
            {
                var player = await service.GetPlayerByIdAsync(id);
                return player is not null ? Results.Ok(player) : Results.NotFound();
            }).WithSummary("Recupera un giocatore dall'ID");

            group.MapGet("/{id:int}/characters", async (int id, ICharacterService characterService) =>
            {

                var characters = await characterService.GetCharactersByPlayerIdAsync(id);
                return Results.Ok(characters);
            }).WithSummary("Recupera tutti i personaggi di un giocatore");
        }
    }
}
