namespace Hydra;

public class QodanaScanManager
{
    private readonly string _linter;
    private readonly string _projectDir;
    private readonly string _resultsDir;
    
    public QodanaScanManager(string linter, string projectDir, string resultsDir)
    {
        _linter = linter;
        _projectDir = projectDir;
        _resultsDir = resultsDir;
    }
    
    public string ResultsDir => _resultsDir;
    
    public void RunScan()
    {
        string command = $"qodana scan --linter {_linter} --project-dir {_projectDir} --results-dir {_resultsDir}";
        ProcessUtility.ExecuteCommand(command);
    }
}