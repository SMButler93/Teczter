using System.Text.RegularExpressions;

namespace Teczter.Domain.ValueObjects;

public record LinkUrl
{
    private string _linkUrl = string.Empty;

    public string LinkURL
    {
        get
        {
            return _linkUrl;
        }
        set
        {
            if (!ValidateLinkUrl(value))
            {
                throw new ArgumentException("URL provided is not considered valid.");
            }

            _linkUrl = value;
        }
    }

    private bool ValidateLinkUrl(string url)
    {
        var regex = "/^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$/;";

        var match = Regex.Match(url, regex, RegexOptions.IgnoreCase);

        return match.Success;
    }
}