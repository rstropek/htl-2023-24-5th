
static class VisitsApi
{
    public static RouteGroupBuilder MapVisitsApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/visits");

        return group;
    }
}

