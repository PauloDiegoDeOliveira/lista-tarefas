namespace Tarefas.API.Configuration
{
    public static class HangfireJobsConfig
    {
        public static void ConfigureRecurringJobs(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            //// Proposta
            //IPropostaJob propostaJob = serviceProvider.GetRequiredService<IPropostaJob>();
            //RecurringJob.AddOrUpdate<IPropostaJob>(
            //    "NotificacoesDePropostaJob",
            //    job => job.RecurringJob(),
            //    Cron.Hourly,
            //    new RecurringJobOptions
            //    {
            //        TimeZone = TimeZoneInfo.Local,
            //    });
        }
    }
}