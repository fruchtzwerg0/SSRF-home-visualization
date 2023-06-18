namespace SSRF_Backend.Services;

using System;
using Microsoft.Extensions.Options;
using SSRF_Backend.Models.Options;

public class CommonService : ICommonService
{
    public CommonService(IOptions<UrlOptions> urlOptions)
    {
        UrlOptions = urlOptions;
    }

    public IOptions<UrlOptions> UrlOptions { get; }
}
