using System.Text.RegularExpressions;

namespace Teczter.Domain.ValueObjects;

public record LinkUrl
{
    private string _url = string.Empty;

    public string Url
    {
        get
        {
            return _url;
        }
        set
        {
            if (!ValidateLinkUrl(value))
            {
                throw new ArgumentException("URL provided is not considered valid.");
            }

            _url = value;
        }
    }

    private bool ValidateLinkUrl(string url)
    {
        var regex = @"^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$/;";

        var match = Regex.Match(url, regex, RegexOptions.IgnoreCase);

        return match.Success;
    }
}