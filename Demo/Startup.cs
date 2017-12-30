using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo
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
            //Register the FastDfsClient repostory
            services.AddSingleton<Zaaby.DFS.Core.IRepository, Zaaby.DFS.FastDfsProvider.Mongo.Repository>(p =>
                new Zaaby.DFS.FastDfsProvider.Mongo.Repository(
                    new Zaaby.DFS.FastDfsProvider.Mongo.MongoDbConfiger(new List<string> {"192.168.5.61:27017"},
                        "FlytOaData", "FlytOaDev", "2016")));

            //Register the FastDfsClient
            services.AddSingleton<Zaaby.DFS.Core.IHandler, Zaaby.DFS.FastDfsProvider.ZaabyFastDfsClient>(p =>
                new Zaaby.DFS.FastDfsProvider.ZaabyFastDfsClient(
                    new List<IPEndPoint> {new IPEndPoint(IPAddress.Parse("192.168.78.152"), 22122)},
                    "group1", services.BuildServiceProvider().GetService<Zaaby.DFS.Core.IRepository>()));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}