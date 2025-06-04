using backend.API.Endpoints;

namespace backend.API.Extensions;

public static class EndpointsMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapPostEndpoints();
        app.MapUserEndpoints();

        return app;
    }
}