using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PocGraphQLHotChocolate.Infra.Context;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Mutations;
using PocGraphQLHotChocolate.Infra.Queries;
using PocGraphQLHotChocolate.Infra.Repository;
using PocGraphQLHotChocolate.Infra.Services;
using PocGraphQLHotChocolate.Infra.Types;

namespace PocGraphQLHotChocolate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(opcoes => opcoes.UseInMemoryDatabase(databaseName: "Books"),ServiceLifetime.Transient);
            #region GraphQL Services
            services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddType<AuthorType>()
                .AddType<BookType>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .Create());
            #endregion

            #region Configuração do Cors CrossOrigins
            services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                });
            #endregion

            #region Repository
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            #endregion

            #region Services
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                #region habilita o ambiente do teste
                app.UsePlayground();
                #endregion
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            #region Rota do Granphql
            app.UseGraphQL("/graphql");
            #endregion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}