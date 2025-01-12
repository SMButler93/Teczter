using System.Text.RegularExpressions;

namespace Teczter.Domain.ValueObjects;

public record LinkUrl
{
    private string _url = null!;

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
        var regex = @"^(https?:\/\/|www\.)[^\s\/$.?#].[^\s]*\.(com|co\.uk|[a-z]{2,})(\/[^\s]*)?$";

        var match = Regex.Match(url, regex, RegexOptions.IgnoreCase);

        return match.Success;
    }
}