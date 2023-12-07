
using System.Text.RegularExpressions;
using Fundraising.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

static partial class VisitsApi
{
    public static IEndpointRouteBuilder MapVisitsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/campaigns/{id}/visits", GetVisits);
        endpoints.MapPost("/campaigns/{id}/visits", AddVisit);
        endpoints.MapPatch("/visits/{id}", PatchVisit);

        return endpoints;
    }

    private static IResult GetVisits(ApplicationDbContext dbContext, [AsParameters] VisitsParameters filter)
    {
        var result = dbContext.Visits
            .Where(v => v.CampaignId == filter.Id)
            .Where(v => filter.Town == null || v.Household!.TownName.Contains(filter.Town))
            .Where(v => filter.Street == null || v.Household!.StreetName.Contains(filter.Street))
            .Where(v => !filter.OnlyUnsuccessful || !v.SuccessfullyVisited)
            .Select(v => new
            {
                v.Household!.TownName,
                v.Household!.StreetName,
                v.Household!.HouseNumber,
                v.Household!.FamilyName,
                v.SuccessfullyVisited,
            });
        return Results.Ok(result);
    }

    private static async Task<IResult> AddVisit(ApplicationDbContext dbContext, int id, VisitDto newVisit)
    {
        if (newVisit.HouseholdId == null && newVisit.Household == null)
        {
            return Results.BadRequest();
        }

        if (newVisit.HouseholdId != null && !await dbContext.Households.AnyAsync(h => h.Id == newVisit.HouseholdId))
        {
            return Results.BadRequest();
        }

        if (newVisit.Household != null && !HousenumberRegex().IsMatch(newVisit.Household.HouseNumber))
        {
            return Results.BadRequest();
        }

        var visit = new Visit
        {
            CampaignId = id,
            SuccessfullyVisited = newVisit.SuccessfullyVisited,
        };
        if (newVisit.HouseholdId == null)
        {
            visit.Household = new Household
            {
                TownName = newVisit.Household!.TownName,
                StreetName = newVisit.Household.StreetName,
                HouseNumber = newVisit.Household.HouseNumber,
                FamilyName = newVisit.Household.FamilyName,
            };
        }
        else
        {
            visit.HouseholdId = newVisit.HouseholdId.Value;
        }

        await dbContext.Visits.AddAsync(visit);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/visits/{visit.Id}", new
        { 
            visit.Id,
            visit.HouseholdId,
            visit.SuccessfullyVisited,
        });
    }

    private static async Task<IResult> PatchVisit(ApplicationDbContext dbContext, int id, PatchVisitDto visit)
    {
        if (!visit.SuccessfullyVisited)
        {
            return Results.BadRequest();
        }

        await dbContext.Visits
            .Where(v => v.Id == id)
            .ExecuteUpdateAsync(v => v.SetProperty(x => x.SuccessfullyVisited, visit.SuccessfullyVisited));
    	/*
        var visitToUpdate = await dbContext.Visits.FindAsync(id);
        if (visitToUpdate == null)
        {
            return Results.NotFound();
        }

        visitToUpdate.SuccessfullyVisited = visit.SuccessfullyVisited;
        */
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    [GeneratedRegex(@"^[1-9]\d*\w?$")]
    private static partial Regex HousenumberRegex();
}

record VisitsParameters(
    [FromRoute] int Id,
    [FromQuery] string? Town,
    [FromQuery] string? Street,
    [FromQuery] bool OnlyUnsuccessful = false
);

record HouseholdDto(
    string TownName,
    string StreetName,
    string HouseNumber,
    string FamilyName
);

record VisitDto(
    int? HouseholdId,
    HouseholdDto? Household,
    bool SuccessfullyVisited
);

record PatchVisitDto(
    bool SuccessfullyVisited
);