using System;

namespace Core.Specifications;

public class ProductSpecParams
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int pageSize = 6;
    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands ?? new List<string>();
        set
        {
            _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                           .ToList();
        }
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types ?? new List<string>();
        set
        {
            _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                         .ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string? Search
    {
        get => _search ?? string.Empty;
        set => _search = value?.ToLower();
    }


}
