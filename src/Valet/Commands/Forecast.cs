using System.CommandLine;
namespace Valet.Commands;

public class Forecast : BaseCommand
{
    private readonly string[] _args;

    public Forecast(string[] args)
    {
        _args = args;
    }

    protected override string Name => "forecast";
    protected override string Description => "Forecasts GitHub actions usage from historical pipeline utilization.";

    private static readonly Option<DateTime> StartDate = new("--start-date", getDefaultValue: () => DateTime.Now.AddDays(-7))
    {
        Description = "The start date of the forecast analysis in YYYY-MM-DD format.",
        IsRequired = false,
    };

    private static readonly Option<DateTime> EndDate = new("--end-date", getDefaultValue: () => DateTime.Now)
    {
        Description = "The end date of the forecast analysis in YYYY-MM-DD format.",
        IsRequired = false,
    };

    private static readonly Option<int> TimeSlice = new("--time-slice", getDefaultValue: () => 60)
    {
        Description = "The time slice in seconds to use for computing concurrency metrics.",
        IsRequired = false,
    };

    private static readonly Option<FileInfo[]> SourceFilePath = new("--source-file-path")
    {
        Description = "The file path(s) to existing jobs data.",
        IsRequired = false,
    };

    protected override Command GenerateCommand(App app)
    {
        var command = base.GenerateCommand(app);

        command.AddGlobalOption(
            new Option<DirectoryInfo>(new[] { "--output-dir", "-o" })
            {
                IsRequired = true,
                Description = "The location for any output files."
            }
        );

        command.AddGlobalOption(StartDate);
        command.AddGlobalOption(EndDate);
        command.AddGlobalOption(TimeSlice);
        command.AddGlobalOption(SourceFilePath);

        command.AddCommand(new AzureDevOps.Forecast(_args).Command(app));

        return command;
    }
}