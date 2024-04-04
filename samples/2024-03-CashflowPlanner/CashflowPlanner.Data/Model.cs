using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashflowPlanner.Data;

[Index("Name", IsUnique = true)]
public class Category
{
    public int ID { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public char InOut { get; set; }
}

[Index("Name", IsUnique = true)]
public class Currency
{
    public int ID { get; set; }

    [MaxLength(3)]
    public string Name { get; set; } = string.Empty;
}

[Index("FromCurrency", "ToCurrency", IsUnique = true)]
public class ExchangeRate
{     
    public int ID { get; set; }

    public string FromCurrency { get; set; } = string.Empty;

    public string ToCurrency { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Rate { get; set; }
}

public class  Cashflow
{
    public int ID { get; set; }

    public short Year { get; set; }

    public byte Month { get; set; }

    public int CategoryID { get; set; }

    public Category? Category { get; set; }

    public string Currency { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
}