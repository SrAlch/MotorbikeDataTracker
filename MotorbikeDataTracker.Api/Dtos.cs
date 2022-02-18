using System;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeDataTracker.Api.Dtos
{
    public record MotorbikeDto(Guid Id, string Brand, int Year, string Model, string Trim);

    //TODO: Add on Range the end date as a DateTime obj
    public record CreateMotorbikeDto([Required] string Brand, [Range(1885, 2022)] int Year, [Required] string Model, string Trim);
    public record UpdateMotorbikeDto([Required] string Brand, [Range(1885, 2022)] int Year, [Required] string Model, string Trim);
}