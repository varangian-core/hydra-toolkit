namespace Hydra;

public class QodanaScanRunner
{
    private readonly QodanaScanManager _scanManager;
    private readonly QodanaResultParser _resultParser;

    public QodanaScanRunner(string linter, string projectDir, string resultsDir)
    {
        _scanManager = new QodanaScanManager(linter, projectDir, resultsDir);
        _resultParser = new QodanaResultParser();
    }

    public IEnumerable<QodanaIssueWithCode> RunScanAndGetIssuesWithCode()
    {
        _scanManager.RunScan();

        string resultsFilePath =
            Path.Combine(_scanManager.ResultsDir, "qodana-short.sarif.json"); //need to fix this later
        var issues = _resultParser.ParseResults(resultsFilePath);

        var issuesWithCode = issues.Select(issue => new QodanaIssueWithCode
        {
            Type = issue.Type,
            Description = issue.Description,
            FilePath = issue.FilePath,
            LineNumber = issue.LineNumber,
            CodeSnippet = GetCodeSnippet(issue.FilePath, issue.LineNumber)
        });

        return issuesWithCode;
    }

    public string GetCodeSnippet(string filePath, int lineNumber)
    {
        var lines = File.ReadAllLines(filePath);
        int contextLines = 2;
        int startLine = Math.Max(lineNumber - contextLines - 1, 0);
        int endLine = Math.Min(lineNumber + contextLines, lines.Length);

        return string
            .Join(Environment.NewLine, lines
                .Skip(startLine));
    }
}

public class QodanaIssueWithCode : QodanaIssue
{
    public string CodeSnippet { get; set; }
}