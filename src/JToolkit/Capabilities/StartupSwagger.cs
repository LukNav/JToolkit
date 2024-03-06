namespace JToolkit.Capabilities;

public static class StartupSwagger
{
    public static void UseSwagger(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}