using Newtonsoft.Json;

namespace Hydra;

public class QodanaResultParser
{
    public IEnumerable<QodanaIssue> ParseResults(string resultFilePath)
    {
        var issues = new List<QodanaIssue>();
        var json = File.ReadAllText(resultFilePath);
        dynamic results = JsonConvert.DeserializeObject(json);

        foreach (var issue in results.issue)
        {
            issues.Add(new QodanaIssue
            {
                Type = issue.type,
                Description = issue.description,
                FilePath = issue.filePath,
                LineNumber = issue.line,
            });
        }


        return issues;
    }
}

public class QodanaIssue
{
    public string Type { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public int LineNumber { get; set; }
}