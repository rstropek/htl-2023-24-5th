using Fundraising.Data;
using Microsoft.EntityFrameworkCore;

static class CampaignApi
{
    public static RouteGroupBuilder MapCampaignApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/campaigns");

        group.MapPost("/", AddCampaign);
        group.MapPatch("/{id}", UpdateCampaign);
        group.MapDelete("/{id}", DeleteCampaign);

        return group;
    }

    private static async Task<IResult> AddCampaign(CampaignDto newCampaign, ApplicationDbContext dbContext)
    {
        // var campaign = new Campaign { Name = newCampaign.Name };
        // await dbContext.Campaigns.AddAsync(campaign);
        // await dbContext.SaveChangesAsync();
        
        var campaign = await dbContext.CreateCampaign(newCampaign.Name);

        return Results.Created($"/campaigns/{campaign.Id}", new { campaign.Id, campaign.Name });
    }

    private static async Task<IResult> UpdateCampaign(int id, CampaignDto updateCampaign, ApplicationDbContext dbContext)
    {
        var campaign = await dbContext.Campaigns.FindAsync(id);
        if (campaign is null)
        {
            return Results.NotFound();
        }

        campaign.Name = updateCampaign.Name;
        await dbContext.SaveChangesAsync();

        return Results.Ok(new { campaign.Id, campaign.Name });
    }

    private static async Task<IResult> DeleteCampaign(int id, ApplicationDbContext dbContext)
    {
        var campaign = await dbContext.Campaigns.FindAsync(id);
        if (campaign is null)
        {
            return Results.NotFound();
        }

        if (await dbContext.Visits.AnyAsync(v => v.CampaignId == id))
        {
            return Results.BadRequest("Cannot delete a campaign that has visits.");
        }

        dbContext.Campaigns.Remove(campaign);
        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    }
}

record CampaignDto(string Name);