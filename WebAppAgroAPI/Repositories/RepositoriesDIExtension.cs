namespace WebAppAgroAPI.Repositories
{
    public static class RepositoriesDIExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) //convert this method to method of the IServiceCollection
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
