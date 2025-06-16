using backend.API.Endpoints;

namespace backend.API.Config;

public static class EndpointsMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapPostEndpoints();
        app.MapUserEndpoints();

        return app;
    }
}