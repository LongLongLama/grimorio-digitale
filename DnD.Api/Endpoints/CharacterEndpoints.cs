using DnD.Domain.DTOs;
using DnD.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DnD.Api.Endpoints
{
    public static class CharacterEndpoints
    {
        public static void MapCharacterEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/characters").WithTags("Characters");

            group.MapGet("/", async (ICharacterService service) =>
            {
                var characters = await service.GetAllCharactersAsync();
                return Results.Ok(characters);
            }).WithSummary("Recupera tutti i personaggi");

            group.MapGet("/{id:int}", async (int id, ICharacterService service) =>
            {
                var character = await service.GetCharacterByIdAsync(id);
                return character is not null ? Results.Ok(character) : Results.NotFound();
            }).WithSummary("Recupera un personaggio per ID");

            group.MapPost("/", async (CharacterCreationDto dto, ICharacterService service) =>
            {
                var newCharacter = await service.CreateCharacterAsync(dto);
                if (newCharacter == null)
                {
                    return Results.BadRequest("Uno o più ID forniti (Player, Race, Class) non sono validi.");
                }
                return Results.Created($"/api/characters/{newCharacter.Id}", newCharacter);
            }).WithSummary("Crea un nuovo personaggio");

            group.MapPut("/{id:int}/name", async (int id, [FromQuery] string newName, ICharacterService service) =>
            {
                var updatedCharacter = await service.UpdateCharacterNameAsync(id, newName);
                return updatedCharacter is not null ? Results.Ok(updatedCharacter) : Results.NotFound();
            }).WithSummary("Aggiorna il nome di un personaggio (passato in query string)");

            group.MapDelete("/{id:int}", async (int id, ICharacterService service) =>
            {
                var success = await service.DeleteCharacterAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            }).WithSummary("Cancella un personaggio");

            group.MapPost("/{id:int}/equip/{itemId:int}/{slot}", async (int id, int itemId, string slot, ICharacterService service) =>
            {
                var success = await service.EquipItemAsync(id, itemId, slot);
                return success ? Results.Ok("Oggetto equipaggiato.") : Results.NotFound("Personaggio, oggetto non trovato, o slot non valido.");
            }).WithSummary("Equipaggia un oggetto a un personaggio");

            group.MapDelete("/{id:int}/equip/{slot}", async (int id, string slot, ICharacterService service) =>
            {
                var success = await service.UnequipItemAsync(id, slot);
                return success ? Results.NoContent() : Results.NotFound();
            }).WithSummary("Rimuove un oggetto equipaggiato");
        }
    }
}
