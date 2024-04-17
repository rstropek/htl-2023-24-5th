namespace CityCongestionCharge.Api;

public static class PaymentsApi
{
    public static RouteGroupBuilder MapPaymentsApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/payments");

        group.MapGet("", GetPayments).Produces<IEnumerable<PaymentResultDto>>();
        group.MapGet("with-detections", GetPaymentsWithDetections).Produces<IEnumerable<PaymentWithDetectionDto>>();
        group.MapPost("", AddPayment).Produces<PaymentResultDto>(StatusCodes.Status201Created).Produces(StatusCodes.Status404NotFound);
        group.MapGet("{id}", GetPaymentById).WithName(nameof(GetPaymentById))
            .Produces<PaymentResultDto>().Produces(StatusCodes.Status404NotFound);

        return group;
    }

    #region Data transfer objects
    /// <summary>
    /// DTO describing a payment
    /// </summary>
    public class PaymentResultDto
    {
        public int Id { get; set; }
        public DateTime PaidForDate { get; set; }
        public decimal PaidAmount { get; set; }
        public string? PayingPerson { get; set; }
        public PaymentType PaymentType { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }

    /// <summary>
    /// Converts a given <see cref="Payment"/> into a <see cref="PaymentResultDto"/>
    /// </summary>
    private static PaymentResultDto PaymentToResultDto(Payment p)
        => new()
        {
            Id = p.Id,
            PaidAmount = p.PaidAmount,
            PayingPerson = p.PayingPerson,
            PaidForDate = p.PaidForDate,
            PaymentType = p.PaymentType,
            LicensePlate = p.Car!.LicensePlate,
        };

    public class DetectionDetailDto
    {
        public DateTime Taken { get; set; }

        public bool MultipleCarsOnOneDetection { get; set; }
    }

    public class PaymentWithDetectionDto
    {
        public int PaymentId { get; set; }

        public decimal PaidAmount { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public IEnumerable<DetectionDetailDto>? DetectionDetails { get; set; }
    }

    public class PaymentAddDto
    {
        public decimal PaidAmount { get; set; }
        public string? PayingPerson { get; set; }
        public PaymentType PaymentType { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
    #endregion

    /// <summary>
    /// Gets a filtered list of payments
    /// </summary>
    /// <param name="context">Data context</param>
    /// <param name="paymentTypeFilter">Optional filter for <see cref="Payment.PaymentType"/></param>
    /// <param name="onlyFuturePayments">Optional filter for getting only payments regarding days in the future</param>
    /// <param name="onlyAnonymous">Optional filter for getting only anonymous payments (i.e. payments without <see cref="Payment.PayingPerson"/>)</param>
    /// <returns>
    /// List of payments fulfilling the given filter parameters.
    /// </returns>
    public static IResult GetPayments(
        CccDataContext context,
        [FromQuery(Name = "type")] PaymentType? paymentTypeFilter,
        [FromQuery(Name = "future")] bool? onlyFuturePayments,
        [FromQuery(Name = "anonym")] bool? onlyAnonymous)
    {
        return Results.Ok(context.FilteredPayments(paymentTypeFilter, onlyFuturePayments, onlyAnonymous)
            .Select(p => PaymentToResultDto(p)));
    }

    /// <summary>
    /// Gets a list of payments for days with detections
    /// </summary>
    /// <param name="context">Data context</param>
    /// <returns>
    /// List of payments for days with car detections
    /// </returns>
    public static IResult GetPaymentsWithDetections(CccDataContext context)
    {
        return Results.Ok(context.Payments
            .Where(p => p.Car!.Detections.Any(d => d.Taken.Year == p.PaidForDate.Year
                && d.Taken.Month == p.PaidForDate.Month && d.Taken.Day == p.PaidForDate.Day))
            .OrderBy(p => p.Car!.LicensePlate)
            .Select(p => new PaymentWithDetectionDto
            {
                PaymentId = p.Id,
                LicensePlate = p.Car!.LicensePlate,
                PaidAmount = p.PaidAmount,
                DetectionDetails = p.Car.Detections
                    .OrderBy(d => d.Taken)
                    .Select(d => new DetectionDetailDto
                    {
                        Taken = d.Taken,
                        MultipleCarsOnOneDetection = d.DetectedCars.Count > 1
                    })
            }));
    }

    /// <summary>
    /// Adds a payment
    /// </summary>
    /// <param name="context">Data context</param>
    /// <param name="p">Payment to add</param>
    /// <returns>Data about the created payment (including primary key)</returns>
    /// <response code="201">Payment created</response>
    /// <response code="404">No car with given license plate exists</response>
    public static async Task<IResult> AddPayment(CccDataContext context, [FromBody]PaymentAddDto p)
    {
        var car = await context.Cars.FirstOrDefaultAsync(c => c.LicensePlate == p.LicensePlate);
        if (car == null) { return Results.NotFound(); }

        var payment = new Payment
        {
            PaidAmount = p.PaidAmount,
            PaidForDate = DateTime.Today,
            PayingPerson = p.PayingPerson,
            PaymentType = p.PaymentType,
            Car = car
        };
        context.Payments.Add(payment);
        await context.SaveChangesAsync();
        return Results.CreatedAtRoute(nameof(GetPaymentById), new { id = payment.Id }, PaymentToResultDto(payment));
    }

    /// <summary>
    /// Get a single payment by ID
    /// </summary>
    /// <param name="context">Data context</param>
    /// <param name="id">Id of the payment to get</param>
    /// <response code="200">Payment found</response>
    /// <response code="404">Payment not found</response>
    public static async Task<IResult> GetPaymentById(CccDataContext context, int id)
    {
        var payment = await context.Payments.AsNoTracking()
            .Include(p => p.Car)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (payment == null) { return Results.NotFound(); }
        return Results.Ok(PaymentToResultDto(payment));
    }
}