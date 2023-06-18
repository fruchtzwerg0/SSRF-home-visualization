namespace SSRF_Backend.Services;

using System;
using Microsoft.Extensions.Options;
using SSRF_Backend.Models.Options;

public interface ICommonService
{
    IOptions<UrlOptions> UrlOptions { get; }
}

