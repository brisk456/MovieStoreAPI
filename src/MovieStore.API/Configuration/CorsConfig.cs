namespace MovieStore.API.Configuration
{
    public static class CorsConfig
    {
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {

            });
        }

        public static void UseCorsConfiguration(this IApplicationBuilder app)
        {

        }

    }
}
