using Kamoul.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor services
builder.Services.AddMudServices();

var app = builder.Build();

// Log startup information
Console.WriteLine($"Application starting in {app.Environment.EnvironmentName} mode");
Console.WriteLine($"Content root: {app.Environment.ContentRootPath}");
Console.WriteLine($"Web root: {app.Environment.WebRootPath}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Add detailed error handling for production debugging
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";
            
            var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            
            await context.Response.WriteAsync($@"
                <html>
                <head><title>Error</title></head>
                <body>
                    <h1>An error occurred</h1>
                    <h2>Exception: {exception?.GetType().Name}</h2>
                    <p><strong>Message:</strong> {exception?.Message}</p>
                    <p><strong>Stack Trace:</strong></p>
                    <pre>{exception?.StackTrace}</pre>
                </body>
                </html>");
        });
    });
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Add security headers globally
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    
    // Remove or customize the Server header for security
    if (context.Response.Headers.ContainsKey("Server"))
    {
        context.Response.Headers.Remove("Server");
    }
    context.Response.Headers.Append("Server", "WebServer");
    
    await next();
});

// Configure static file caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Add security headers
        ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        
        // Debug: Log static file requests
        Console.WriteLine($"Serving static file: {ctx.File.Name} - Status: {ctx.Context.Response.StatusCode}");
        
        // Debug: Log image file serving
        if (ctx.File.Name.EndsWith(".png") || ctx.File.Name.EndsWith(".ico") || ctx.File.Name.EndsWith(".jpg") || ctx.File.Name.EndsWith(".jpeg"))
        {
            Console.WriteLine($"Image file served: {ctx.File.Name} - Content-Type: {ctx.Context.Response.ContentType}");
        }
        
        // Debug: Log CSS file serving
        if (ctx.File.Name.EndsWith(".css"))
        {
            Console.WriteLine($"CSS file served: {ctx.File.Name} - Content-Type: {ctx.Context.Response.ContentType}");
        }
        
        // Cache static files for 1 year in production
        if (!app.Environment.IsDevelopment())
        {
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
        }
        else
        {
            // No caching in development
            ctx.Context.Response.Headers.Append("Cache-Control", "no-cache,must-revalidate");
            ctx.Context.Response.Headers.Append("Expires", "0");
        }
    }
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
