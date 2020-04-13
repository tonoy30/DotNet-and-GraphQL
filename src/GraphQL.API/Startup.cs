using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.API.Graphql.Queries;
using GraphQL.API.Models;
using GraphQL.API.Repository;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GraphQL.API
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
            TypeConversion.Default.Register<string, ObjectId>(from => ObjectId.Parse(from));
            TypeConversion.Default.Register<ObjectId, string>(from => from.ToString());
            // setup the repositories
            services.AddSingleton<IMongoClient>(new MongoClient("mongodb://127.0.0.1:27017"));
            services.AddSingleton<IMongoDatabase>(s => s.GetRequiredService<IMongoClient>().GetDatabase("PagingDemo"));
            services.AddSingleton<IMongoCollection<Message>>(s =>
                s.GetRequiredService<IMongoDatabase>().GetCollection<Message>("messages"));
            services.AddSingleton<IMongoCollection<User>>(s =>
                s.GetRequiredService<IMongoDatabase>().GetCollection<User>("users"));
            services.AddSingleton<MessageRepository>();
            services.AddSingleton<UserRepository>();

            services.AddDataLoaderRegistry();

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<QueryType>()
                    .AddMutationType<MutationType>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseGraphQLHttpPost(new HttpPostMiddlewareOptions {Path = "/graphql"})
                .UseGraphQLHttpGetSchema(new HttpGetSchemaMiddlewareOptions {Path = "/graphql/schema"});
        }
    }
}