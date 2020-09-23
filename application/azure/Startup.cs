using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace azure
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    string usernamePath = Environment.GetEnvironmentVariable("DEMO_USERNAME_PATH");
                    string passwordPath = Environment.GetEnvironmentVariable("DEMO_PASSWORD_PATH");
                    if( (string.IsNullOrEmpty(usernamePath)) || (string.IsNullOrEmpty(passwordPath)) ){
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("DEMO_USERNAME_PATH or DEMO_PASSWORD_PATH environment variables were not set.");
                        return;
                    }

                    string username;
                    string password;
                    string outputSuccess = "";
                    SecretHub.Client client;

                    try {
                        // Let's create a new client first
                        client = new SecretHub.Client();
                    } catch(Exception ex) {
                        Console.WriteLine(ex.ToString());
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Error encountered while creating client.");
                        return;
                    }

                    try {
                        // Before doing anything, let's check whether the username and password secrets exists
                        if (!client.Exists(usernamePath))
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("Username secret does not exist.");
                            return;
                        }
                        if (!client.Exists(passwordPath))
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync("Password secret does not exist.");
                            return;
                        }

                        // Then we read the two secrets.
                        username = client.ReadString(usernamePath);
                        password = client.ReadString(passwordPath);

                        outputSuccess += "Hello "+username+"!\n";
                    } catch(Exception ex) {
                        Console.WriteLine(ex.ToString());
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Error encountered while reading secrets.");
                        return;
                    }

                    await context.Response.WriteAsync(outputSuccess);
                });
            });
        }
    }
}
