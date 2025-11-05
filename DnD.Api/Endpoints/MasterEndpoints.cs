using DnD.Domain.Services;

namespace DnD.Api.Endpoints
{
    public static class MasterEndpoints
    {
        public static void MapMasterEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/master").WithTags("Master Actions");

            group.MapPost("/levelup", async (IMasterService service) =>
            {
                var updatedCharacters = await service.LevelUpAllCharactersAsync();
                if (updatedCharacters.Count == 0)
                {
                    return Results.Ok("Nessun personaggio da far salire di livello.");
                }
                return Results.Ok(updatedCharacters);
            }).WithSummary("Fa salire di livello tutti i personaggi");
        }
    }
}
