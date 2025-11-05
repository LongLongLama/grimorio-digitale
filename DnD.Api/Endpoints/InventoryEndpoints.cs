using DnD.Domain.Services;

namespace DnD.Api.Endpoints
{
    public static class InventoryEndpoints
    {
        public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/inventory").WithTags("Inventory");

            group.MapGet("/{characterId:int}", async (int characterId, IInventoryService service) =>
            {
                var inventory = await service.GetInventoryAsync(characterId);
                return Results.Ok(inventory);
            }).WithSummary("Recupera l'inventario di un personaggio");

            group.MapPost("/{characterId:int}/add/{itemId:int}", async (int characterId, int itemId, IInventoryService service) =>
            {
                var success = await service.AddItemToInventoryAsync(characterId, itemId);
                return success
                    ? Results.Ok("Oggetto aggiunto con successo.")
                    : Results.BadRequest("Personaggio o oggetto non trovato.");
            }).WithSummary("Aggiunge un oggetto all'inventario");

            group.MapDelete("/{characterId:int}/remove/{itemId:int}", async (int characterId, int itemId, IInventoryService service) =>
            {
                var success = await service.RemoveItemFromInventoryAsync(characterId, itemId);
                return success
                    ? Results.NoContent()
                    : Results.NotFound("Oggetto non trovato nell'inventario del personaggio.");
            }).WithSummary("Rimuove un oggetto dall'inventario");
        }
    }
}
