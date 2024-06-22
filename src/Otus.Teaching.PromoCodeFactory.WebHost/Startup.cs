using Otus.Teaching.PromoCodeFactory.DataAccess.Extensions;

namespace Otus.Teaching.PromoCodeFactory.WebHost;

public class Startup(
    IConfiguration conf
    ) {

    public void ConfigureServices(IServiceCollection services) {
        string dbCnn = conf.GetValue<string>("CnnString") ?? throw new ArgumentNullException("No CnnString");
        services
            .ConfigureDbContext(dbCnn)
            .AddControllers();

        services
            .AddOpenApiDocument(options => {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseHsts();
        }

        app.UseOpenApi();
        app.UseSwaggerUi3(x => {
            x.DocExpansion = "list";
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}