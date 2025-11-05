using DnD.Domain.Services;

namespace DnD.Api.Endpoints
{
    public static class WikiEndpoints
    {
        public static void MapWikiEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/wiki").WithTags("Wiki");

           
            group.MapGet("/races", async (IWikiService service) =>
            {
                var races = await service.GetAllRacesAsync();
                return Results.Ok(races);
            }).WithSummary("Recupera tutte le razze");


            group.MapGet("/races/{id:int}", async (int id, IWikiService service) =>
            {
                var race = await service.GetRaceDetailsAsync(id);
                return race is not null ? Results.Ok(race) : Results.NotFound();
            }).WithSummary("Recupera i dettagli di una singola razza");


            group.MapGet("/classes", async (IWikiService service) =>
            {
                var classes = await service.GetAllClassesAsync();
                return Results.Ok(classes);
            }).WithSummary("Recupera tutte le classi");

            group.MapGet("/classes/{id:int}", async (int id, IWikiService service) =>
            {
                var charClass = await service.GetClassDetailsAsync(id);
                return charClass is not null ? Results.Ok(charClass) : Results.NotFound();
            }).WithSummary("Recupera i dettagli di una singola classe");


            group.MapGet("/spells", async (IWikiService service) =>
            {
                var spells = await service.GetAllSpellsAsync();
                return Results.Ok(spells);
            }).WithSummary("Recupera tutti gli incantesimi");

            group.MapGet("/spells/{id:int}", async (int id, IWikiService service) =>
            {
                var spell = await service.GetSpellDetailsAsync(id);
                return spell is not null ? Results.Ok(spell) : Results.NotFound();
            }).WithSummary("Recupera i dettagli di un singolo incantesimo");
        }
    }
}
