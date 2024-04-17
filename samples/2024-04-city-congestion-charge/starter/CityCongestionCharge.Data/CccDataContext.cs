using Microsoft.EntityFrameworkCore;

namespace CityCongestionCharge.Data;

public partial class CccDataContext
{
    /// <summary>
    /// Return a filtered list of cars
    /// </summary>
    /// <param name="carType">Optional car type filter (filters on <see cref="Car.CarType"/>)</param>
    /// <param name="licensePlateFilter">Optional license plate filter (filters on <see cref="Car.LicensePlate"/>)</param>
    /// <param name="onlyInside">
    ///     If <c>true</c>, only detections with type 
    ///     <see cref="Detection.MovementType"/> = <see cref="MovementType.DrivingInside"/> should be returned
    /// </param>
    /// <param name="onlyMultipleCars">Only detections containing more than one car should be returned</param>
    /// <remarks>
    /// <paramref name="licensePlateFilter"/> is an empty string if the list should not be filtered by
    /// license plate number.
    /// </remarks>
    public IQueryable<Detection> FilteredDetections(CarType? carType, string licensePlateFilter, 
        bool? onlyInside, bool? onlyMultipleCars)
    {
        throw new NotImplementedException();
    }
}
