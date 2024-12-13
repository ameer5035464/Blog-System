using BlogSystem.DAL.Contracts;

namespace BlogSystem.PL.Extensions
{
    public static class IntializeData
    {
        public async static Task<WebApplication> DataIntialize(this WebApplication app)
        {
            using var createRequest = app.Services.CreateAsyncScope();
            var providers = createRequest.ServiceProvider;
            var IdentityProvider = providers.GetRequiredService<IDbIdentityIntializer>();
            var dataProvider = providers.GetRequiredService<IDbDataIntializer>();

            try
            {

                await dataProvider.UpdateAllDatabaseAsync();
                await dataProvider.SeedDataAsync();

                await IdentityProvider.UpdateAllDatabaseAsync();
                await IdentityProvider.SeedDataAsync();
            }
            catch (Exception)
            {

                throw;
            }


            return app;
        }
    }
}
